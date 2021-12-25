using IdentityAPI.Extension.Installers;
using IdentityAPI.Servises;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Twilio.Clients;

namespace IdentityAPI.Installers
{
    /// <summary>
    ///     Register all application services
    /// </summary>
    public class ServicesInstaller : IInstaller
    {
        public void InstallerServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddHttpClient<ITwilioRestClient, TwilioClient>();
        }
    }
}