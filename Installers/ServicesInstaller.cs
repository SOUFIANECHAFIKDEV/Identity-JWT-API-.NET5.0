using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityAPI.Data;
using IdentityAPI.Domain;
using IdentityAPI.Extension.Installers;
using IdentityAPI.Servises;

namespace IdentityAPI.Installers
{
    /// <summary>
    ///     Register all application services
    /// </summary>
    public class ServicesInstaller : IInstaller
    {
        public void InstallerServices(IServiceCollection services, IConfiguration Configuration)
        {
            //services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<DataContext>();
            //services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddScoped<IIdentityService, IdentityService>();
        }
    }
}