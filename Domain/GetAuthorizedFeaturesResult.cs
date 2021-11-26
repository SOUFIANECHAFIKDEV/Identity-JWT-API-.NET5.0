using System.Collections.Generic;

namespace IdentityAPI.Domain
{
    public class GetAuthorizedFeaturesResult
    {
        public List<FeatureAuthorization> FeaturesAuthorizations{ get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
