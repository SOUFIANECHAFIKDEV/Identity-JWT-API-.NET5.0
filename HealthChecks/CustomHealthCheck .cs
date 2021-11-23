using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityAPI.HealthCheck
{

    public class CustomHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                //Do health check

                return Task.FromResult(HealthCheckResult.Healthy());
            }
            catch (Exception exception)
            {

                return Task.FromResult(HealthCheckResult.Unhealthy(exception.Message));
            }
        }
    }
}