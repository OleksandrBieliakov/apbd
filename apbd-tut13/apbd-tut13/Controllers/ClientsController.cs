using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apbd_tut13.DAL;
using apbd_tut13.DTOs.Requests;
using apbd_tut13.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apbd_tut13.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public ClientsController(ShopDbContext context, IDbService dbService)
        {
            _dbService = dbService;
            _dbService.DbContext = context;
        }

        [HttpGet("{idClient}/orders")]
        public IActionResult AddOrder(int idClient, AddOrderRequest request)
        {
            int result = _dbService.AddOrder(idClient, request);
            if (result == 1)
            {
                return BadRequest("The Customer does not exist");
            }
            if (result == 2)
            {
                return BadRequest("The product does not exist");
            }
            return Ok();
        }

    }
}