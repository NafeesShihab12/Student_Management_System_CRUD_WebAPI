using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagementSystem.Service.DTO;
using StudentManagementSystem.Repository.Domain;
using System.Reflection;
using System.Net;

namespace StudentManagementSystem.Service.Mapper
{
    public static class StudentMapper
    {
        public static StudentDTO MapToDTO(Student student)
        {
            if (student == null)
                return null;

            return new StudentDTO
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                DOB = student.DOB,
                Gender = student.Gender,
                CGPA = student.CGPA,
                Address = student.Address,
                Email = student.Email
            };
        }

        public static Student MapToEntity(StudentDTO studentDTO)
        {
            if (studentDTO == null)
                return null;

            return new Student
            {
                Id = studentDTO.Id,
                FirstName = studentDTO.FirstName,
                LastName = studentDTO.LastName,
                DOB = studentDTO.DOB,
                Gender = studentDTO.Gender,
                CGPA = studentDTO.CGPA,
                Address = studentDTO.Address,
                Email = studentDTO.Email
            };
        }
    }
}

