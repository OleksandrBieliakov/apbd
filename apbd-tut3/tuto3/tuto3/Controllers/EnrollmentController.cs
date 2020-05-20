using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tuto3.DAL;
using tuto3.DTOs.Requests;
using tuto3.Entities;
using tuto3.Models;

namespace tuto3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IDbService _dbService;
        private readonly StudentContext _studentContext;


        public EnrollmentController(IDbService dbService, StudentContext studentContext)
        {
            _dbService = dbService;
            _studentContext = studentContext;
        }

        [HttpPost]
        //[Authorize(Roles = "employee")]
        //public IActionResult EnrollStudent(StudentEnrollmentReq request)
        [AllowAnonymous]
        public IActionResult EnrollStudent(EnrollStudentReq request)
        {
            /*
            var response = _dbService.EnrollStudent(request);
            if (response.Error != null)
            {
                return BadRequest(response.Error);
            }
            */

            var idStudy = _studentContext.Studies.Where(s => s.Name == request.Studies).Select(s => s.IdStudy).FirstOrDefault();
            if (idStudy == 0) // default
            {
                return BadRequest("Study not found");
            }

            var idEnrollment = _studentContext.Enrollment.Where(e => e.IdStudy == idStudy && e.Semester == 1).Select(e => e.IdEnrollment).FirstOrDefault();
            if (idEnrollment == 0) // default
            {
                idEnrollment = _studentContext.Enrollment.Select(e => e.IdEnrollment).Max() + 1;
                var enrollment = new Entities.Enrollment
                {
                    IdEnrollment = idEnrollment,
                    IdStudy = idStudy,
                    Semester = 1,
                    StartDate = DateTime.Now
                };
                _studentContext.Add(enrollment);
            }

            var student = new Entities.Student
            {
                IndexNumber = request.IndexNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                RefreshToken = request.Password,
                BirthDate = request.BirthDate,
                IdEnrollment = idEnrollment,
                IdRole = request.IdRole,
            };
            _studentContext.Add(student);
            _studentContext.SaveChanges();

            var response = new
            {
                IdEnrollment = idEnrollment
            };
            return Ok(response);
        }

        [HttpGet("{idEnrollment}")]
        public IActionResult GetEnrollment(int idEnrollment)
        {
            Models.Enrollment enrollment = _dbService.GetEnrollment(idEnrollment);
            if (enrollment == null)
            {
                return NotFound("Enrollment not found");
            }
            return Ok(enrollment);
        }

        [HttpPost("promotions")]
        //[Authorize(Roles = "employee")]
        [AllowAnonymous]
        public IActionResult PromoteStudents(StudnetsPromotionReq request)
        {
            /*
            var response = _dbService.PromoteStudnets(request);
            if (response == null)
            {
                return BadRequest();
            }
            if (response.Error != null)
            {
                return NotFound(response.Error);
            }
            return Created($"api/enrollments/{response.IdEnrollment}", response);
            */
            var idStudy = _studentContext.Studies.Where(s => s.Name == request.Studies).Select(s => s.IdStudy).FirstOrDefault();
            if (idStudy == 0) // default
            {
                return BadRequest("Study not found");
            }

            var idEnrollment = _studentContext.Enrollment.Where(e => e.IdStudy == idStudy && e.Semester == request.Semester).Select(e => e.IdEnrollment).FirstOrDefault();
            if (idEnrollment == 0) // default
            {
                return BadRequest("No such enrollment");
            }

            var idEnrollmentNext = _studentContext.Enrollment.Where(e => e.IdStudy == idStudy && e.Semester == request.Semester + 1).Select(e => e.IdEnrollment).FirstOrDefault();
            if (idEnrollmentNext == 0) // default
            {
                idEnrollmentNext = _studentContext.Enrollment.Select(e => e.IdEnrollment).Max() + 1;
                var enrollment = new Entities.Enrollment
                {
                    IdEnrollment = idEnrollmentNext,
                    IdStudy = idStudy,
                    Semester = request.Semester + 1,
                    StartDate = DateTime.Now
                };
                _studentContext.Add(enrollment);
            }

            var students = _studentContext.Student.Where(s => s.IdEnrollment == idEnrollment)
                .Select(s => new Entities.Student
                {
                    IndexNumber = s.IndexNumber,
                    IdEnrollment = idEnrollmentNext
                }).ToList();
            foreach (var student in students)
            {
                _studentContext.Attach(student);
                _studentContext.Entry(student).Property("IdEnrollment").IsModified = true;
            }
            _studentContext.SaveChanges();
            return Created($"api/enrollments/{idEnrollmentNext}", idEnrollmentNext);
        }

    }
}