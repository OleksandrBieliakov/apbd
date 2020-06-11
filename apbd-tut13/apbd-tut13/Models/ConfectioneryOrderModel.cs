using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut13.Models
{
    public class ConfectioneryOrderModel
    {
        public int IdConfectionery { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal PricePerItem { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
    }
}
