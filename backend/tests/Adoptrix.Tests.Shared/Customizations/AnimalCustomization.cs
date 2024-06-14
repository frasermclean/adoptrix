using Adoptrix.Domain.Models;

namespace Adoptrix.Tests.Shared.Customizations;

public class AnimalCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Animal>(composer => composer
            .With(animal => animal.Name, SampleData.RandomName)
            .With(animal => animal.DateOfBirth,
                DateOnly.FromDateTime(DateTime.Now - TimeSpan.FromDays(365 * Random.Shared.Next(1, 15)))));
    }
}
