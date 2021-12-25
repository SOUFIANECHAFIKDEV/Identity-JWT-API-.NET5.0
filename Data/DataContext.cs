using IdentityAPI.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityAPI.Data.EntityConfigurations.Security;
using IdentityAPI.Data.EntityConfigurations.References;

namespace IdentityAPI.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<LegalStatus> LegalStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UsersConfiguration());
            builder.ApplyConfiguration(new RolesConfiguration());
            builder.ApplyConfiguration(new UserRolesConfiguration());
            builder.ApplyConfiguration(new UserClaimsConfiguration());
            builder.ApplyConfiguration(new UserLoginsConfiguration());
            builder.ApplyConfiguration(new UserRoleClaimsConfiguration());
            builder.ApplyConfiguration(new UserTokensConfiguration());
            builder.ApplyConfiguration(new RefreshTokenConfiguration());
            builder.ApplyConfiguration(new CityConfiguration());
            builder.ApplyConfiguration(new LegalStatusConfiguration());
        }
    }
}