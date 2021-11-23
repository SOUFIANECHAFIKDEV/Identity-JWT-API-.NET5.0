using System;
using System.Collections.Generic;

namespace IdentityAPI.Contracts.HealthChecks
{
    public class HealthCheckResponse
    {
        public string Status { get; set; }
        public IEnumerable<HealthChecks> Checks { get; set; }
        public TimeSpan Duration { get; set; }
    }
}