using apbd_tut13.DTOs.Requests;
using apbd_tut13.Entities;
using apbd_tut13.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut13.DAL
{
    public interface IDbService
    {
        public ShopDbContext DbContext { get; set; }

        public IEnumerable<OrderModel> GetOrders(CustomersOrdersRequest request);
        public int AddOrder(int idClient, AddOrderRequest request);
    }
}
