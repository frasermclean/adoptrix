using Adoptrix.Database.Converters;
using Adoptrix.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Database.Configuration;

public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.HasAlternateKey(animal => animal.Slug);

        builder.Property(animal => animal.Name)
            .HasColumnType("nvarchar")
            .HasMaxLength(Animal.NameMaxLength);

        builder.Property(animal => animal.Sex)
            .HasColumnType("char")
            .HasMaxLength(1)
            .HasConversion<SexConverter>();

        builder.Property(animal => animal.CreatedAt)
            .HasColumnType("datetime2")
            .HasPrecision(2)
            .HasDefaultValueSql("getutcdate()");

        builder.Property(animal => animal.Slug)
            .HasColumnType("nvarchar")
            .HasMaxLength(Animal.SlugMaxLength)
            .HasDefaultValueSql("newid()");

        var animalImagesBuilder = builder.OwnsMany(animal => animal.Images)
            .ToTable("AnimalImages");

        ConfigureAnimalImagesTable(animalImagesBuilder);
    }

    private static void ConfigureAnimalImagesTable(OwnedNavigationBuilder<Animal, AnimalImage> builder)
    {
        builder.Property(imageInformation => imageInformation.UploadedAt)
            .HasPrecision(2)
            .HasDefaultValueSql("getutcdate()");

        builder.Property(imageInformation => imageInformation.OriginalFileName)
            .HasColumnType("nvarchar")
            .HasMaxLength(512);

        builder.Property(imageInformation => imageInformation.OriginalContentType)
            .HasColumnType("varchar")
            .HasMaxLength(AnimalImage.ContentTypeMaxLength);
    }
}
