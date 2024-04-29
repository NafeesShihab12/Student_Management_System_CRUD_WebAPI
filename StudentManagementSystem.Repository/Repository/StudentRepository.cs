using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Repository.Data;
using StudentManagementSystem.Repository.Domain;

namespace StudentManagementSystem.Repository.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentDbContext _context;

        public StudentRepository(StudentDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            return await _context.Student.ToListAsync();
        }

        public async Task<Student> GetStudentById(int id)
        {
            return await _context.Student.FindAsync(id);
        }

        public async Task<Student> AddStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            await _context.Student.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task DeleteStudent(int id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
                throw new InvalidOperationException($"Student with ID {id} not found.");

            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}
