using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut11.Models
{
    public partial class Prescription
    {
        public Prescription()
        {
            PrescriptionMedicament = new HashSet<PrescriptionMedicament>();
        }

        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public int IdPatient { get; set; }
        public int IdDoctor { get; set; }

        public virtual Doctor IdDoctorNavigation { get; set; }
        public virtual Patient IdPatientNavigation { get; set; }
        public virtual ICollection<PrescriptionMedicament> PrescriptionMedicament { get; set; }
    }
}
