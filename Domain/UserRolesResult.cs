using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace IdentityAPI.Domain
{
    public class UserRolesResult
    {
        public List<IdentityRole> Roles { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}