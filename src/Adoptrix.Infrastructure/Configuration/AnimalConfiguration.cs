using Adoptrix.Domain;
using Adoptrix.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Infrastructure.Configuration;

public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.Property(animal => animal.Name)
            .HasColumnType("nvarchar")
            .HasMaxLength(Animal.NameMaxLength);

        builder.Property(animal => animal.Species)
            .HasColumnName("SpeciesCode")
            .HasColumnType("char")
            .HasMaxLength(SpeciesCodeConverter.CodeLength)
            .HasConversion<SpeciesCodeConverter>();
    }
}