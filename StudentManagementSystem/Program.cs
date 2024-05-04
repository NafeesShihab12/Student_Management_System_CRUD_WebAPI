using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StudentManagementSystem.Repository.Data;
using StudentManagementSystem.Service.Mapper;
using StudentManagementSystem.Service.Service;
using StudentManagementSystem.Repository.Repository;
using StudentManagementSystem.API.Middlewares;
using StudentManagementSystem.API.Filters;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore.Storage;
using StudentManagementSystem.Service;

namespace StudentManagementSystem.API
{
    public class Program
    {
        public static object services;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            Dependency.RegisterServices(builder.Services);

            // Configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            builder.Configuration.AddConfiguration(configuration);

            // Configure database context based on environment
            if (builder.Configuration["DatabaseProvider"] == "PostgreSQL")
            {
                builder.Services.AddDbContext<StudentDbContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("StudentManagementSystemPostgres")));
            }
            else if (builder.Configuration["DatabaseProvider"] == "InMemory")
            {
                builder.Services.AddDbContext<StudentDbContext>(options =>
                    options.UseInMemoryDatabase("Student"));
            }

            // Register services
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<StudentMapper>();
            builder.Services.AddScoped<AuthorizationFilter>();
            builder.Services.AddScoped<GlobalExceptionFilter>();
            builder.Services.AddScoped<JwtTokenValidator>();
            builder.Services.AddResponseCaching();

            // JWT authentication configuration
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Management System API v1"));
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            // Enable CORS
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://example.com") // Specify allowed origins
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<AuthenticationMiddleware>();

            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}
