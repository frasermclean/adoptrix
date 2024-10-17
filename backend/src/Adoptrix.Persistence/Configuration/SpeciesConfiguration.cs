using Adoptrix.Core;
using Adoptrix.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Persistence.Configuration;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.Property(species => species.Name)
            .HasMaxLength(Species.NameMaxLength);

        builder.Property(species => species.LastModifiedUtc)
            .HasPrecision(2)
            .HasDefaultValueSql("getutcdate()")
            .HasConversion<UtcDateTimeConverter>();

        builder.HasIndex(species => species.Name)
            .IsUnique();
    }
}
