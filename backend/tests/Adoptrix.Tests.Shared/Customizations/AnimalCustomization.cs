using Adoptrix.Domain.Models;

namespace Adoptrix.Tests.Shared.Customizations;

public class AnimalCustomization : ICustomization
{
    private static readonly string[] Names = ["Buddy", "Max", "Bella", "Lucy", "Charlie", "Daisy", "Bailey", "Molly"];
    private const int MaxAgeInYears = 10;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<Animal>(composer => composer
            .With(animal => animal.Name, Names[Random.Shared.Next(Names.Length)])
            .With(animal => animal.DateOfBirth,
                DateOnly.FromDateTime(DateTime.Now - TimeSpan.FromDays(365 * Random.Shared.Next(1, MaxAgeInYears)))));
    }
}
