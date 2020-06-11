using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut13.Entities
{
    public partial class Order
    {
        public Order()
        {
            ConfectioneryOrder = new HashSet<ConfectioneryOrder>();
        }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int IdOrder { get; set; }
        public DateTime DateAccepted { get; set; }
        public DateTime DateFinished { get; set; }
        public string Notes { get; set; }
        public int IdClient { get; set; }
        public int IdEmployee { get; set; }

        public virtual Customer IdCustomerNavigation { get; set; }
        public virtual Employee IdEmployeeNavigation { get; set; }

        public virtual ICollection<ConfectioneryOrder> ConfectioneryOrder { get; set; }
    }
}
