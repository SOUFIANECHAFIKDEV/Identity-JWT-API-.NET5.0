using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IdentityAPI.Domain;

namespace IdentityAPI.Data.EntityConfigurations.Security
{
    public class UsersConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("users", "security");
            builder.Ignore(e => e.PhoneNumberConfirmed);
        }
    }
}