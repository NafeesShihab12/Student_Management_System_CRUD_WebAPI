using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementSystem.Repository.Repository;

namespace StudentManagementSystem.Repository
{
    public static class Dependency
    {
        public static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IStudentRepository, StudentRepository>();
        }
    }
}
