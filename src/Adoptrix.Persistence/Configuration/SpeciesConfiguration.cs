using Adoptrix.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Persistence.Configuration;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.Property(species => species.Name)
            .HasMaxLength(Species.NameMaxLength);

        builder.Property(species => species.CreatedAt)
            .HasPrecision(2)
            .HasDefaultValueSql("getutcdate()");

        builder.Property(animal => animal.CreatedBy)
            .HasDefaultValue(Guid.Empty);

        builder.HasKey(species => species.Name);

        builder.HasData(InitialData);
    }

    internal static readonly Species[] InitialData =
    [
        new Species { Name = "Dog" },
        new Species { Name = "Cat" },
        new Species { Name = "Bird" }
    ];
}
