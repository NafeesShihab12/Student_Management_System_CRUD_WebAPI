using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementSystem.Service.Service;
using StudentManagementSystem.Repository.Repository;

namespace StudentManagementSystem.Service
{
    public static class Dependency
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentService>();
        }
    }
}
