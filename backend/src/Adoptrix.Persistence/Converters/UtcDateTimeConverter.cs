using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Adoptrix.Persistence.Converters;

public class UtcDateTimeConverter() : ValueConverter<DateTime, DateTime>(
    static datetime => datetime.ToUniversalTime(),
    static datetime => DateTime.SpecifyKind(datetime, DateTimeKind.Utc));
