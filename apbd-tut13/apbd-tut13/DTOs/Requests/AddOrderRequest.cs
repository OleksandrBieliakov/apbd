using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut13.DTOs.Requests
{
    public class AddOrderRequest
    {
        public DateTime DateAccepted { get; set; }
        public string Notes { get; set; }
        public IEnumerable<AddOrderRequestConfectionery> Confectionery { get; set; }
    }
}
