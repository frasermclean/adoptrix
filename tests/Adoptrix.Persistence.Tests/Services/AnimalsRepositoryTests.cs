using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Initializer;
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
        var request = new SearchAnimalsRequest { Name = "Alberto" };

        // act
        var matches = await animalsRepository.SearchAsync(request);
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
        var breed = await breedsRepository.GetByIdAsync(SeedData.Breeds["German Shepherd"].Id);
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

    [Fact]
    public async Task DeleteAsync_WithValidAnimal_ShouldPass()
    {
        // arrange
        var breed = await breedsRepository.GetByIdAsync(SeedData.Breeds["Golden Retriever"].Id);
        var animalToAdd = AnimalFactory.Create(breed: breed);
        await animalsRepository.AddAsync(animalToAdd);

        // act
        await animalsRepository.DeleteAsync(animalToAdd);
        var animal = await animalsRepository.GetByIdAsync(animalToAdd.Id);

        // assert
        animal.Should().BeNull();
    }
}
