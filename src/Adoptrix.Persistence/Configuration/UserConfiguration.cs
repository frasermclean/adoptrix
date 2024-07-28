using Adoptrix.Core;
using Adoptrix.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Persistence.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Ignore(user => user.FirstName)
            .Ignore(user => user.LastName)
            .Ignore(user => user.DisplayName)
            .Ignore(user => user.EmailAddress);

        builder.Property(user => user.Role)
            .HasConversion<UserRoleConverter>();

        builder.Property(user => user.CreatedAt)
            .HasColumnType("datetime2")
            .HasPrecision(2)
            .HasDefaultValueSql("getutcdate()");
    }
}
