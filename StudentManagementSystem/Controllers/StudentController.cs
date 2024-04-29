using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Service.DTO;
using StudentManagementSystem.Service.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudents();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDTO>> GetStudentById(int id)
        {
            try
            {
                var student = await _studentService.GetStudentById(id);
                if (student == null)
                    return NotFound();

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<StudentDTO>> AddStudent(StudentDTO studentDTO)
        {
            try
            {
                var addedStudent = await _studentService.AddStudent(studentDTO);
                return CreatedAtAction(nameof(GetStudentById), new { id = addedStudent.Id }, addedStudent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StudentDTO>> UpdateStudent(int id, StudentDTO studentDTO)
        {
            try
            {
                if (id != studentDTO.Id)
                    return BadRequest("ID mismatch");

                await _studentService.UpdateStudent(studentDTO);
                return Ok(studentDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                await _studentService.DeleteStudent(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
