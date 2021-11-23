using IdentityAPI.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityAPI.Extension.Installers;
using IdentityAPI.HealthCheck;

namespace IdentityAPI.Installers
{
    public class HealthChecksInstaller : IInstaller
    {
        public void InstallerServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddHealthChecks()
                    .AddDbContextCheck<DataContext>()
                    .AddCheck<CustomHealthCheck>("ustom"); // 'Custom' is the component name
        }
    }
}