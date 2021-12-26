using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAPI.Contracts.V1.Requests
{
    public class SendResetPasswordRequest
    {
        public string Email { get; set; }
    }
}
