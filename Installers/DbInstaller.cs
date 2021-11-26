using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityAPI.Data;
using IdentityAPI.Extension.Installers;
using IdentityAPI.Domain;

namespace IdentityAPI.Installers
{
    public class DbInstaller : IInstaller
    {
        /// <summary>
        /// Setting up the application database
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public void InstallerServices(IServiceCollection services, IConfiguration Configuration)
        {
            //configure the database Connection String
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //configure identity default tables
            services.AddDefaultIdentity<ApplicationUser>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.SignIn.RequireConfirmedEmail = false;
                config.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext>();
        }
    }
}
