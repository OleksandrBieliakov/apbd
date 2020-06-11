using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut13.Entities
{
    public partial class Employee
    {
        public Employee()
        {
            Order = new HashSet<Order>();
        }

        public int IdEmployee { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}
