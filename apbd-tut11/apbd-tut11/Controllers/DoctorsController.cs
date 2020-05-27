using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apbd_tut11.DAL;
using apbd_tut11.DTOs.Requests;
using apbd_tut11.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_tut11.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public DoctorsController(ClinicDbContext context, IDbService dbService)
        {
            _dbService = dbService;
            _dbService.DbContext = context;

        }
            
        [HttpGet]
        public IActionResult GetDoctors()
        {
            return Ok(_dbService.GetDoctors());
        }

        [HttpPost]
        public IActionResult AddDoctor(AddDoctorRequest request)
        {
            _dbService.AddDoctor(request);
            return Ok();
        }

        [HttpDelete("{idDoctor}")]
        public IActionResult DeleteDoctor(int idDoctor)
        {
            _dbService.DeleteDoctor(idDoctor);
            return Ok();
        }

        [HttpPost("modify")]
        public IActionResult UpdateDoctor(UpdateDoctorRequest request)
        {
            _dbService.UpdateDoctor(request);
            return Ok();
        }
        
    }
}