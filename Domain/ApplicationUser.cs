using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace IdentityAPI.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int LegalStatusId { get; set; }
        public LegalStatus LegalStatus { get; set; }
    }
}