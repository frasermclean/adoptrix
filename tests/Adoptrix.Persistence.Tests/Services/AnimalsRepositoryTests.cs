using Adoptrix.Persistence.Services;
using Adoptrix.Persistence.Tests.Fixtures;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Persistence.Tests.Services;

[Trait("Category", "Integration")]
[Collection(nameof(DatabaseCollection))]
public class AnimalsRepositoryTests
{
    private readonly IAnimalsRepository animalsRepository;
    private readonly IBreedsRepository breedsRepository;

    public AnimalsRepositoryTests(DatabaseFixture fixture)
    {
        var collection = fixture.GetRepositoryCollection();
        (animalsRepository, breedsRepository, _) = collection;
    }

    [Fact]
    public async Task SearchAsync_WithValidRequest_ShouldPass()
    {
        // arrange
        const string name = "Alberto";

        // act
        var matches = await animalsRepository.SearchAsync(name);

        // assert
        matches.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldFail()
    {
        // arrange
        const int animalId = -1;

        // act
        var animal = await animalsRepository.GetAsync(animalId);

        // assert
        animal.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WithValidAnimal_ShouldPass()
    {
        // arrange
        var breed = await breedsRepository.GetByIdAsync(1);
        var animalToAdd = AnimalFactory.Create(breed: breed);

        // act
        await animalsRepository.AddAsync(animalToAdd);
        var animal = await animalsRepository.GetAsync(animalToAdd.Id);

        // assert
        animal.Should().NotBeNull();
        animal!.Id.Should().BePositive();
        animal.Name.Should().Be(animalToAdd.Name);
        animal.Description.Should().Be(animalToAdd.Description);
        animal.Breed.Should().Be(breed);
        animal.Sex.Should().Be(animalToAdd.Sex);
        animal.DateOfBirth.Should().Be(animalToAdd.DateOfBirth);
        animal.CreatedBy.Should().NotBeEmpty();
    }

    [Fact]
    public async Task DeleteAsync_WithValidAnimal_ShouldPass()
    {
        // arrange
        var breed = await breedsRepository.GetByIdAsync(1);
        var animalToAdd = AnimalFactory.Create(breed: breed);
        await animalsRepository.AddAsync(animalToAdd);

        // act
        await animalsRepository.DeleteAsync(animalToAdd);
        var animal = await animalsRepository.GetAsync(animalToAdd.Id);

        // assert
        animal.Should().BeNull();
    }
}
