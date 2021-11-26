using System.Collections.Generic;

namespace IdentityAPI.Domain
{
    public class ApplicationFeatures
    {
        public string AppModule { get; set; }
        public List<string> Features { get; set; }
    }
}