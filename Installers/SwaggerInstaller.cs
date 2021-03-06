using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IdentityAPI.Extension.Installers;

namespace IdentityAPI.Installers
{
    /// <summary>
    /// Setting up Swagger for Api documentation
    /// </summary>
    public class SwaggerInstaller : IInstaller
    {
        public void InstallerServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSwaggerGen(x =>
            {
                //ToDO get version from targeted URL
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "User Identity API", Version = "v1" });

                x.ExampleFilters();

                #region Setting up JWT support (Authentication)

                x.AddSecurityDefinition(name: "Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }/*,
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,*/
                        },
                        new List<string>()
                    }
                });

                #endregion

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                x.IncludeXmlComments(xmlPath);
            });

            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
        }
    }
}
