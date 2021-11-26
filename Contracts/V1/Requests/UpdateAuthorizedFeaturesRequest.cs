using System.Collections.Generic;

namespace IdentityAPI.Contracts.V1.Requests
{
    public class UpdateAuthorizedFeaturesRequest
    {
        public string UserId { get; set; }
        public List<string> AppFeatures { get; set; }
    }
}