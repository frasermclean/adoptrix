using Adoptrix.Core;
using Adoptrix.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Persistence.Configuration;

public class AuditEntryConfiguration : IEntityTypeConfiguration<AuditEntry>
{
    public void Configure(EntityTypeBuilder<AuditEntry> builder)
    {
        builder.Property(entry => entry.StartTimeUtc)
            .HasPrecision(2)
            .HasConversion<UtcDateTimeConverter>();

        builder.Property(entry => entry.EndTimeUtc)
            .HasPrecision(2)
            .HasConversion<UtcDateTimeConverter>();

        builder.Property(entry => entry.OperationName)
            .HasMaxLength(AuditEntry.OperationNameMaxLength);

        builder.Property(entry => entry.ErrorMessage)
            .HasMaxLength(AuditEntry.ErrorMessageMaxLength);

        builder.Property(entry => entry.Metadata)
            .HasMaxLength(AuditEntry.MetadataMaxLength);

        builder.HasIndex(entry => entry.UserId);
        builder.HasIndex(entry => entry.OperationName);
    }
}
