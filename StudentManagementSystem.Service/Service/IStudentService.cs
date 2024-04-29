using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagementSystem.Service.DTO;

namespace StudentManagementSystem.Service.Service
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDTO>> GetAllStudents();
        Task<StudentDTO> GetStudentById(int id);
        Task<StudentDTO> AddStudent(StudentDTO student);
        Task<StudentDTO> UpdateStudent(StudentDTO student);
        Task DeleteStudent(int id);
    }
}
