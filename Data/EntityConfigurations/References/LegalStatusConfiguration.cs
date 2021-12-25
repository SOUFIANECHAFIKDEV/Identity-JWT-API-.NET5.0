using IdentityAPI.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityAPI.Data.EntityConfigurations.References
{
    public class LegalStatusConfiguration : IEntityTypeConfiguration<LegalStatus>
    {
        public void Configure(EntityTypeBuilder<LegalStatus> builder)
        {
            builder.ToTable("LegalStatus", "references");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasMany(p => p.Users).WithOne(x => x.LegalStatus).HasForeignKey(x => x.LegalStatusId);
            builder.Property(p => p.Name);

            //Seed data
            builder.HasData(
                new City { Id = 1, Name = "Individual" },
                new City { Id = 2, Name = "Entreprise" });
        }
    }
}