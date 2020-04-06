using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tuto3.DAL;
using tuto3.DTOs.Requests;
using tuto3.Models;

namespace tuto3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IDbService _dbService;

        public EnrollmentController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public IActionResult EnrollStudent(StudentEnrollmentReq request)
        {
            var response = _dbService.EnrollStudent(request);
            if (response.Error != null)
            {
                return BadRequest(response.Error);
            }
            return Ok(response);
        }

        [HttpGet("{idEnrollment}")]
        public IActionResult GetEnrollment(int idEnrollment)
        {
            Enrollment enrollment = _dbService.GetEnrollment(idEnrollment);
            if (enrollment == null)
            {
                return NotFound("Enrollment not found");
            }
            return Ok(enrollment);
        }

        [HttpPost("promotions")]
        public IActionResult PromoteStudents(StudnetsPromotionReq request)
        {
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
        }


        
    }
}