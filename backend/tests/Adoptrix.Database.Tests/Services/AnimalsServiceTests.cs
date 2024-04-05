using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using Adoptrix.Database.Tests.Fixtures;
using Adoptrix.Domain.Models.Factories;

namespace Adoptrix.Database.Tests.Services;

[Trait("Category", "Integration")]
[Collection(nameof(DatabaseCollection))]
public class AnimalsServiceTests(DatabaseFixture fixture)
{
    private readonly IAnimalsService animalsService = fixture.AnimalsRepository!;
    private readonly IBreedsRepository breedsRepository = fixture.BreedsRepository!;

    [Fact]
    public async Task GetAsync_WithInvalidId_ShouldFail()
    {
        // arrange
        var animalId = Guid.Empty;

        // act
        var result = await animalsService.GetAsync(animalId);

        // assert
        result.Should().BeFailure().Which
            .Should().HaveReason<AnimalNotFoundError>($"Could not find animal with ID {animalId}");
    }

    [Fact]
    public async Task AddAsync_WithValidAnimal_ShouldPass()
    {
        // arrange
        var breedResult = await breedsRepository.GetByIdAsync(BreedIds.GermanShepherd);
        var animal = AnimalFactory.Create(breed: breedResult.Value);

        // act
        var addResult = await animalsService.AddAsync(animal);
        var getResult = await animalsService.GetAsync(animal.Id);

        // assert
        addResult.Should().BeSuccess().Which.Should().HaveValue(animal);
        getResult.Should().BeSuccess().Which.Value.Should().BeEquivalentTo(animal);
    }
}
