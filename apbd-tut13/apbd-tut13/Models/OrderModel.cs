using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut13.Models
{
    public class OrderModel
    {
        public int IdOrder { get; set; }
        public DateTime DateAccepted { get; set; }
        public DateTime DateFinished { get; set; }
        public string Notes { get; set; }
        public IEnumerable<ConfectioneryOrderModel> Basket { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
