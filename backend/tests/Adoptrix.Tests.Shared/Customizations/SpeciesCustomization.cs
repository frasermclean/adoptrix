using Adoptrix.Domain.Models;

namespace Adoptrix.Tests.Shared.Customizations;

public class SpeciesCustomization : ICustomization
{
    private static readonly string[] Names = ["Dog", "Cat", "Bird", "Fish", "Rabbit", "Hamster"];

    public void Customize(IFixture fixture)
    {
        fixture.Customize<Species>(composer => composer
            .With(animal => animal.Name, Names[Random.Shared.Next(Names.Length)]));
    }
}
