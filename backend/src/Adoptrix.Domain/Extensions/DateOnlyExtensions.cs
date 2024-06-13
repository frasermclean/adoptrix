namespace Adoptrix.Domain.Extensions;

public static class DateOnlyExtensions
{
    public static string ToAgeString(this DateOnly dob)
    {
        var now = DateTime.UtcNow;
        var months = now.Month - dob.Month;
        var years = now.Year - dob.Year;

        if (now.Day < dob.Day)
        {
            months--;
        }

        if (months < 0)
        {
            years--;
            months += 12;
        }

        return $"{years} year{(years == 1 ? "" : "s")}, {months} month{(months == 1 ? "" : "s")}";
    }
}
