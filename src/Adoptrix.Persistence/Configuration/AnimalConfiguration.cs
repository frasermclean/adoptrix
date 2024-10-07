using Adoptrix.Core;
using Adoptrix.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Persistence.Configuration;

public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.Property(animal => animal.Id)
            .HasDefaultValueSql("newid()");

        builder.Property(animal => animal.Name)
            .HasMaxLength(Animal.NameMaxLength);

        builder.Property(animal => animal.Description)
            .HasMaxLength(Animal.DescriptionMaxLength);

        builder.Property(animal => animal.Sex)
            .HasConversion<SexConverter>();

        builder.Property(animal => animal.LastModifiedUtc)
            .HasPrecision(2)
            .HasDefaultValueSql("getutcdate()");

        builder.Property(animal => animal.LastModifiedBy)
            .HasDefaultValue(Guid.Empty);

        builder.Property(animal => animal.Slug)
            .HasMaxLength(Animal.SlugMaxLength);

        builder.HasAlternateKey(animal => animal.Slug);

        builder.OwnsMany(animal => animal.Images, imagesBuilder =>
        {
            imagesBuilder.ToTable("AnimalImages");

            imagesBuilder.WithOwner()
                .HasForeignKey(animalImage => animalImage.AnimalSlug)
                .HasPrincipalKey(animal => animal.Slug);

            imagesBuilder.Property(animalImage => animalImage.Id)
                .HasDefaultValueSql("newid()");

            imagesBuilder.Property(animalImage => animalImage.LastModifiedUtc)
                .HasPrecision(2)
                .HasDefaultValueSql("getutcdate()");

            imagesBuilder.Property(animalImage => animalImage.OriginalFileName)
                .HasMaxLength(512);

            imagesBuilder.Property(animalImage => animalImage.OriginalContentType)
                .HasMaxLength(AnimalImage.ContentTypeMaxLength);
        });
    }
}
