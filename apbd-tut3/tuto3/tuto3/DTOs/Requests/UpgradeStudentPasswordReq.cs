using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tuto3.DTOs.Requests
{
    public class UpgradeStudentPasswordReq
    {
        public string IndexNumber { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
    }
}
