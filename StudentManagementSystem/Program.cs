using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementSystem.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using StudentManagementSystem.Repository.Data;
using StudentManagementSystem.Service.Mapper;
using StudentManagementSystem.Service.Service;
using StudentManagementSystem.Repository.Repository;
using StudentManagementSystem.API.Middlewares;

namespace StudentManagementSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            CreateHostBuilder(args).Build();

            app.UseRequestLoggingMiddleware();
            app.UseErrorHandlingMiddleware();

            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;
                var databaseProvider = configuration["DatabaseProvider"];

                if (databaseProvider == "PostgreSQL")
                {
                    var connectionString = configuration.GetConnectionString("DefaultConnection");
                    services.AddDbContext<StudentDbContext>(options =>
                        options.UseNpgsql(connectionString));
                }
                else if (databaseProvider == "InMemory")
                {
                    services.AddDbContext<StudentDbContext>(options =>
                        options.UseInMemoryDatabase("StudentDatabase"));
                }

                Dependency.RegisterServices(services);

                services.AddControllers();
            })

            .Configure(app =>
                                {

                                    app.UseRouting();
                                    app.UseAuthentication();
                                    app.UseAuthorization();
                                    app.UseCors();
                                    app.UseResponseCaching();
                                    app.UseEndpoints(endpoints =>
                                    {
                                        endpoints.MapControllers();
                                    });
                                });
        });


    }
}
