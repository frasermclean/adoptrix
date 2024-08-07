﻿using Adoptrix.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Persistence.Configuration;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.HasIndex(breed => breed.Name)
            .IsUnique();

        builder.Property(breed => breed.Name)
            .HasColumnType("nvarchar")
            .HasMaxLength(Breed.NameMaxLength);

        builder.Property(breed => breed.CreatedAt)
            .HasColumnType("datetime2")
            .HasPrecision(2)
            .HasDefaultValueSql("getutcdate()");
    }
}
