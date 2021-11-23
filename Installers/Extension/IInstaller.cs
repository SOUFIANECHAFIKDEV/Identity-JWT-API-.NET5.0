using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAPI.Extension.Installers
{
    public interface IInstaller
    {
        void InstallerServices(IServiceCollection services, IConfiguration Configuration);
    }
}