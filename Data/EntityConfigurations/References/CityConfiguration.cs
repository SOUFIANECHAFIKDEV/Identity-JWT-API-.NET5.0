using IdentityAPI.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityAPI.Data.EntityConfigurations.References
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("City", "references");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasMany(p => p.Users).WithOne(x => x.City).HasForeignKey(x => x.CityId);
            builder.Property(p => p.Name);

            //Seed data
            builder.HasData(
                new City { Id = 1, Name = "Rabat" },
                new City { Id = 2, Name = "Casablanca" });
        }
    }
}
