using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut11.DTOs.Requests
{
    public class UpdateDoctorRequest
    {
        public int IdDoctor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
