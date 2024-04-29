using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagementSystem.Repository.Domain;

namespace StudentManagementSystem.Repository.Repository
{
    public interface IStudentRepository
    {
        Task<Student> AddStudent(Student student);
        Task<Student> GetStudentById(int id);
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student> UpdateStudent(Student student);
        Task DeleteStudent(int id);

    }
}
