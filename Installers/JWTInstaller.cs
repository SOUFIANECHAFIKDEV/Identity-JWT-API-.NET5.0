using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IdentityAPI.Extension.Installers;
using IdentityAPI.Options;

namespace IdentityAPI.Installers
{
    public class JWTInstaller : IInstaller
    {
        /// <summary>
        ///  Setting up JWT support (Authentication)
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public void InstallerServices(IServiceCollection services, IConfiguration Configuration)
        {
            //fetch configuration Settings from appSettings.json
            var jwtSettings = new JwtSettings();
            Configuration.Bind(key: nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            //setting up the token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.ASCII.GetBytes(jwtSettings.Key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
            services.AddSingleton(tokenValidationParameters);

            // configure Authentication Scheme & JWT
            services.AddAuthentication(configureOptions: x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
            });
        }
    }
}
