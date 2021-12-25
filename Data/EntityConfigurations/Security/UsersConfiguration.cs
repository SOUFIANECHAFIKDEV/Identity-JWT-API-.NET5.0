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
            //builder.Ignore(e => e.PhoneNumberConfirmed);
            builder.Property(p => p.FirstName).IsRequired(false).HasMaxLength(50);
            builder.Property(p => p.LastName).IsRequired(false).HasMaxLength(50);
            builder.Ignore(p => p.UserName);
        }
    }
}