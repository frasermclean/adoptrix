﻿using Adoptrix.Core;
using Adoptrix.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Persistence.Configuration;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.Property(breed => breed.Name)
            .HasMaxLength(Breed.NameMaxLength);

        builder.Property(breed => breed.LastModifiedUtc)
            .HasPrecision(2)
            .HasDefaultValueSql("getutcdate()")
            .HasConversion<UtcDateTimeConverter>();

        builder.HasIndex(breed => breed.Name)
            .IsUnique();
    }
}
