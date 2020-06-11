using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut13.DTOs.Requests
{
    public class AddOrderRequestConfectionery
    {
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
    }
}
