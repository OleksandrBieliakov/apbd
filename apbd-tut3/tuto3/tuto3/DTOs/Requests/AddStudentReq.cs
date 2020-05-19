using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tuto3.DTOs.Requests
{
    public class AddStudentReq
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int IdEnrollment { get; set; }
    }
}
