using IdentityAPI.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityAPI.Data.EntityConfigurations.Security
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken", "security");

            builder.HasKey(p => p.Token);
            builder.Property(p => p.Token).ValueGeneratedOnAdd();
            builder.HasOne(p => p.User).WithMany(x => x.RefreshTokens).HasForeignKey(x => x.UserId);
        }
    }
}