using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tuto3.DAL;
using tuto3.Models;

namespace tuto3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpGet("enrollment/{indexNumber}")]
        public IActionResult GetStudentEnrollment(string indexNumber)
        {
            Enrollment enrollment = _dbService.GetEnrollment(indexNumber);
            if (enrollment == null)
            {
                return NotFound("Student not found");
            }
            return Ok(enrollment);
        }

        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
            Student student = _dbService.GetStudent(indexNumber);
            if (student == null)
            {
                return NotFound("Student not found");
            }
            return Ok(student);
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            int inserted = _dbService.InsertStudent(student);
            return Ok($"Inserted students: {inserted}");
        }

        [HttpDelete("{indexNumber}")]
        public IActionResult DeleteStudent(string indexNumber)
        {
            int deleted = _dbService.DeleteStudnet(indexNumber);
            if (deleted == 0)
            {
                return NotFound("Student not found");
            }
            return Ok($"Students deleted: {deleted}");
        }

        [HttpPut]
        public IActionResult PutStudent(Student student)
        {
            int put = _dbService.InsertOrUpdate(student);
            return Ok($"Inserted or updated students:  {put}");
        }

    }
}