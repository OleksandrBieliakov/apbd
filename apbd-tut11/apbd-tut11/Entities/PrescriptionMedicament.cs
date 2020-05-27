using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut11.Models
{
    public partial class PrescriptionMedicament
    {
        public int IdMedicament { get; set; }
        public int IdPrescription { get; set; }
        public int Dose { get; set; }
        public string Details { get; set; }

        public virtual Medicament IdMedicamentNavigation { get; set; }
        public virtual Prescription IdPrescriptionNavigation { get; set; }
    }
}
