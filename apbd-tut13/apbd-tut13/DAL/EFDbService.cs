using apbd_tut13.DTOs.Requests;
using apbd_tut13.Entities;
using apbd_tut13.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut13.DAL
{
    public class EFDbService : IDbService
    {
        public ShopDbContext DbContext { get; set; }

        public IEnumerable<OrderModel> GetOrders(CustomersOrdersRequest request)
        {
            IEnumerable<OrderModel> orders = null;
            if (request.Name == null && request.Surname == null)
            {
                orders = DbContext.Order.Select(o => new OrderModel
                {
                    IdOrder = o.IdOrder,
                    DateAccepted = o.DateAccepted,
                    DateFinished = o.DateFinished,
                    Notes = o.Notes
                })
                .ToList();
            }
            else
            {
                var res = DbContext.Order.Join(DbContext.Customer, o => o.IdClient, c => c.IdClient, (o, c) => new { o, c });

                if (request.Name == null)
                {
                    res = res.Where(oc => oc.c.Surname == request.Surname);
                }
                else if (request.Surname == null)
                {
                    res = res.Where(oc => oc.c.Name == request.Name);
                }
                else
                {
                    res = res.Where(oc => oc.c.Name == request.Name && oc.c.Surname == request.Surname);
                }

                orders = res.Select(oc => new OrderModel
                {
                    IdOrder = oc.o.IdOrder,
                    DateAccepted = oc.o.DateAccepted,
                    DateFinished = oc.o.DateFinished,
                    Notes = oc.o.Notes
                })
                .ToList();
            }
            foreach (var order in orders)
            {
                var basket = DbContext.ConfectioneryOrder.Join(DbContext.Confectionery, o => o.IdConfectionery, c => c.IdConfectionery, (o, c) => new { o, c })
                    .Where(oc => oc.o.IdOrder == order.IdOrder)
                    .Select(oc => new ConfectioneryOrderModel
                    {
                        IdConfectionery = oc.o.IdConfectionery,
                        Name = oc.c.Name,
                        Type = oc.c.Type,
                        PricePerItem = oc.c.PricePerItem,
                        Quantity = oc.o.Quantity,
                        Notes = oc.o.Notes
                    }).ToList();
                order.Basket = basket;

                decimal totalPrice = 0;
                basket.ForEach(o => totalPrice += o.PricePerItem * o.Quantity);
                order.TotalPrice = totalPrice;
            }
            return orders;
        }

        public int AddOrder(int idClient, AddOrderRequest request)
        {
            int count = DbContext.Customer.Where(c => c.IdClient == idClient).Count();
            if (count == 0)
            {
                return 1; // The Customer does not exist
            }
            int idOrder = DbContext.Order.Select(o => o.IdOrder).Max() + 1;
            DbContext.Add(new Order
            {
                IdOrder = idOrder,
                DateAccepted = request.DateAccepted,
                Notes = request.Notes,
                IdClient = idClient,
                IdEmployee = 1 // default =1; TODO add employee info to the request (not mentioned in the task)
            });
            foreach (var product in request.Confectionery)
            {
                int idProduct = DbContext.Confectionery.Where(c => c.Name == product.Name).Select(c => c.IdConfectionery).FirstOrDefault();
                if (idProduct == 0)
                {
                    return 2; // The product does not exist
                }
                DbContext.Add(new ConfectioneryOrder
                {
                    IdConfectionery = idProduct,
                    Quantity = product.Quantity,
                    Notes = product.Notes,
                    IdOrder = idOrder
                });
            }
            DbContext.SaveChanges();
            return 0;
        }
    }
}
