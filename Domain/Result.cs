using System.Collections.Generic;

namespace IdentityAPI.Domain
{
    public class Result
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}