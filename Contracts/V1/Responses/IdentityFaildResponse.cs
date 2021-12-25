using System.Collections.Generic;

namespace IdentityAPI.Contracts.V1.Responses
{
    public class IdentityFaildResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}