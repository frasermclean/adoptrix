namespace Adoptrix.Tests.Shared.Customizations;

public class TwoYearsAgoDateOnlyCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<DateOnly>(composer => composer.FromFactory(() =>
        {
            var randomYears = TimeSpan.FromDays(365 * Random.Shared.Next(1, 5));
            var randomDays = TimeSpan.FromDays(Random.Shared.Next(1, 365));
            return DateOnly.FromDateTime(DateTime.UtcNow - randomYears - randomDays);
        }));
    }
}
