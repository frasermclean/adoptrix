using Adoptrix.Application.Features.Animals.Commands;

namespace Adoptrix.Tests.Shared.Customizations;

public class AddAnimalCommandCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<AddAnimalCommand>(composer => composer
            .With(command => command.Name, SampleData.RandomName));
    }
}
