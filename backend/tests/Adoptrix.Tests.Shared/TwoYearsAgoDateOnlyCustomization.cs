namespace Adoptrix.Tests.Shared;

public class TwoYearsAgoDateOnlyCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<DateOnly>(composer => composer.FromFactory(() => CreateDateOnly(2)));
    }

    private static DateOnly CreateDateOnly(int yearsAgo)
    {
        var dateTime = DateTime.Now - TimeSpan.FromDays(365 * yearsAgo);
        return DateOnly.FromDateTime(dateTime);
    }
}
