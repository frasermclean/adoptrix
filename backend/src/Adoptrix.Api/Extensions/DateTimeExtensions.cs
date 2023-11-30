namespace Adoptrix.Api.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ToUtc(this DateTime dateTime) => dateTime.Kind switch
    {
        DateTimeKind.Utc => dateTime,
        DateTimeKind.Unspecified => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc),
        _ => dateTime.ToUniversalTime()
    };

}