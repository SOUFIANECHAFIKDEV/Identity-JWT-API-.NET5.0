using System.Collections.Generic;

namespace IdentityAPI.Domain
{
    public class LegalStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}