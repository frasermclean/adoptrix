using Adoptrix.AI.Tests.Fixtures;
using Adoptrix.Application.Features.Generators.Queries;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;

namespace Adoptrix.AI.Tests.Services;

[Trait("Category", "Integration")]
public class AnimalAssistantTests(AiFixture fixture) : IClassFixture<AiFixture>
{
    private readonly IAnimalAssistant animalAssistant = fixture.AnimalAssistant;

    [Fact(Skip = "Test is skipped as it requires authentication")]
    public async Task GetAnimalDescriptionAsync_Should_Return_ExpectedDescription()
    {
        // arrange
        const string animalName = "Bobby";
        const string breedName = "Dachshund";
        const string speciesName = "Dog";
        const Sex sex = Sex.Male;
        var dateOfBirth = new DateOnly(2019, 1, 1);
        var query = new GenerateAnimalDescriptionQuery(animalName, breedName, speciesName, sex, dateOfBirth);

        // act
        var response = await animalAssistant.GenerateDescriptionAsync(query);

        // assert
        response.Description.Should().NotBeNullOrWhiteSpace().And.Contain(animalName);
    }
}
