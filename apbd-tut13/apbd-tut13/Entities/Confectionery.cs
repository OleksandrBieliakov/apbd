using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut13.Entities
{
    public partial class Confectionery
    {
        public Confectionery() 
        {
            ConfectioneryOrder = new HashSet<ConfectioneryOrder>();
        }

        public int IdConfectionery { get; set; }
        public string Name { get; set; }
        public decimal PricePerItem { get; set; }
        public string Type { get; set; }

        public virtual ICollection<ConfectioneryOrder> ConfectioneryOrder { get; set; }

    }
}
