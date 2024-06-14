using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using Adoptrix.Database.Tests.Fixtures;
using Adoptrix.Tests.Shared.Factories;
using AutoFixture.Xunit2;

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
        (animalsRepository, breedsRepository, _) = collection;
    }

    [Theory, AutoData]
    public async Task GetAsync_WithInvalidId_ShouldReturnFailure(Guid animalId)
    {
        // act
        var result = await animalsRepository.GetAsync(animalId);

        // assert
        result.Should().BeFailure().Which
            .HasError<AnimalNotFoundError>(error => error.Message == $"Could not find animal with id: {animalId}");
    }

    [Theory, AutoData]
    public async Task GetAsync_WithInvalidSlug_ShouldReturnFailure(string animalSlug)
    {
        // act
        var result = await animalsRepository.GetAsync(animalSlug);

        // assert
        result.Should().BeFailure().Which
            .HasError<AnimalNotFoundError>(error => error.Message == $"Could not find animal with slug: {animalSlug}");
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

    [Fact]
    public async Task DeleteAsync_WithValidAnimal_ShouldPass()
    {
        // arrange
        var breed = await breedsRepository.GetByIdAsync(BreedIds.GoldenRetriever);
        var animalToAdd = AnimalFactory.Create(breed: breed);
        await animalsRepository.AddAsync(animalToAdd);

        // act
        await animalsRepository.DeleteAsync(animalToAdd);
        var animal = await animalsRepository.GetByIdAsync(animalToAdd.Id);

        // assert
        animal.Should().BeNull();
    }
}
