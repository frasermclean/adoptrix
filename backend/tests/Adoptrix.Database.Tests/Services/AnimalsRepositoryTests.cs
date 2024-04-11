using Adoptrix.Application.Services.Repositories;
using Adoptrix.Database.Tests.Fixtures;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Database.Tests.Services;

[Trait("Category", "Integration")]
[Collection(nameof(DatabaseCollection))]
public class AnimalsRepositoryTests
{
    private readonly IAnimalsRepository animalsRepository;
    private readonly IBreedsRepository breedsRepository;

    public AnimalsRepositoryTests(DatabaseFixture fixture)
    {
        var collection = fixture.GetRepositoryCollection();
        animalsRepository = collection.AnimalsRepository;
        breedsRepository = collection.BreedsRepository;
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldFail()
    {
        // arrange
        var animalId = Guid.Empty;

        // act
        var animal = await animalsRepository.GetByIdAsync(animalId);

        // assert
        animal.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WithValidAnimal_ShouldPass()
    {
        // arrange
        var breed = await breedsRepository.GetByIdAsync(BreedIds.GermanShepherd);
        var animalToAdd = AnimalFactory.Create(breed: breed);

        // act
        await animalsRepository.AddAsync(animalToAdd);
        var animal = await animalsRepository.GetByIdAsync(animalToAdd.Id);

        // assert
        animal.Should().NotBeNull();
        animal!.Id.Should().NotBeEmpty();
        animal.Name.Should().Be(animalToAdd.Name);
        animal.Description.Should().Be(animalToAdd.Description);
        animal.Breed.Should().Be(breed);
        animal.Sex.Should().Be(animalToAdd.Sex);
        animal.DateOfBirth.Should().Be(animalToAdd.DateOfBirth);
        animal.CreatedBy.Should().NotBeEmpty();
    }
}
