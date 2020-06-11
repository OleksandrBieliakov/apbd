using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut13.Entities
{
    public partial class ConfectioneryOrder
    {
        public int IdConfectionery { get; set; }
        public int IdOrder { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }

        public virtual Confectionery IdConfectioneryNavigation { get; set; }
        public virtual Order IdOrderNavigation { get; set; }

    }
}
