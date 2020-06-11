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
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IDbService _dbService;

        public OrdersController(ShopDbContext context, IDbService dbService)
        {
            _dbService = dbService;
            _dbService.DbContext = context;
        }

        [HttpGet]
        public IActionResult GetOrders(CustomersOrdersRequest request)
        {
            return Ok(_dbService.GetOrders(request));
        }

    }
}