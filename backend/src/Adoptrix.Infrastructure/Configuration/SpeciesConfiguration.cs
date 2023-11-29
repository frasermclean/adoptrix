using Adoptrix.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Infrastructure.Configuration;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    private static readonly Species[] SeedData =
    {
        new() { Id = 1, Name = "Dog" },
        new() { Id = 2, Name = "Cat" },
        new() { Id = 3, Name = "Horse" },
    };

    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.HasIndex(species => species.Name)
            .IsUnique();

        builder.Property(species => species.Name)
            .HasColumnType("nvarchar")
            .HasMaxLength(Species.NameMaxLength);

        builder.HasData(SeedData);
    }
}