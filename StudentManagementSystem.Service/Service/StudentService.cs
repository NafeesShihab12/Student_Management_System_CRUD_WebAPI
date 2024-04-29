using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagementSystem.Service.Mapper;
using StudentManagementSystem.Service.Service;
using StudentManagementSystem.Service.DTO;
using StudentManagementSystem.Repository.Domain;
using StudentManagementSystem.Repository.Repository;


namespace StudentManagementSystem.Service.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllStudents()
        {
            var students = await _studentRepository.GetAllStudents();
            return (IEnumerable<StudentDTO>)StudentMapper.MapToDTO((Student)students);
        }

        public async Task<StudentDTO> GetStudentById(int id)
        {
            var student = await _studentRepository.GetStudentById(id);
            return StudentMapper.MapToDTO(student);
        }

        public async Task<StudentDTO> AddStudent(StudentDTO student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            var studentEntity = StudentMapper.MapToEntity(student);
            await _studentRepository.AddStudent(studentEntity);
            return student;
        }

        public async Task<StudentDTO> UpdateStudent(StudentDTO student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            var existingStudent = await _studentRepository.GetStudentById(student.Id);
            if (existingStudent == null)
                throw new ArgumentException("Student not found", nameof(student.Id));

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.DOB = student.DOB;
            existingStudent.Gender = student.Gender;
            existingStudent.CGPA = student.CGPA;
            existingStudent.Address = student.Address;
            existingStudent.Email = student.Email;
            
            await _studentRepository.UpdateStudent(existingStudent);
            return student;
        }

        public async Task DeleteStudent(int id)
        {
            var existingStudent = await _studentRepository.GetStudentById(id);
            if (existingStudent == null)
                throw new ArgumentException("Student not found", nameof(id));

            await _studentRepository.DeleteStudent(id);
        }
    }
}
