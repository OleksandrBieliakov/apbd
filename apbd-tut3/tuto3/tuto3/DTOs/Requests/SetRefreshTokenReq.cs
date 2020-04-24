using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tuto3.DTOs.Requests
{
    public class SetRefreshTokenReq
    {
        public string IndexNumber { get; set; }
        public string RefreshToken { get; set; }
    }
}
