using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tuto3.DTOs.Responses
{
    public class StudentCredentialsRes
    {
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Role { get; set; }
    }
}
