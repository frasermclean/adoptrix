using Adoptrix.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Persistence.Configuration;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.HasIndex(species => species.Name)
            .IsUnique();

        builder.Property(species => species.Name)
            .HasColumnType("nvarchar")
            .HasMaxLength(Species.NameMaxLength);

        builder.Property(species => species.CreatedAt)
            .HasColumnType("datetime2")
            .HasPrecision(2)
            .HasDefaultValueSql("getutcdate()");
    }
}
