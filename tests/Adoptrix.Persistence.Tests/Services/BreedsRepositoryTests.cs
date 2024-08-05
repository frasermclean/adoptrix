using Adoptrix.Core;
using Adoptrix.Initializer;
using Adoptrix.Persistence.Services;
using Adoptrix.Persistence.Tests.Fixtures;
using Adoptrix.Tests.Shared.Factories;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Persistence.Tests.Services;

[Trait("Category", "Integration")]
[Collection(nameof(DatabaseCollection))]
public class BreedsRepositoryTests
{
    private readonly IBreedsRepository breedsRepository;
    private readonly ISpeciesRepository speciesRepository;

    public BreedsRepositoryTests(DatabaseFixture fixture)
    {
        var collection = fixture.GetRepositoryCollection();
        (_, breedsRepository, speciesRepository) = collection;
    }

    [Fact]
    public async Task SearchAsync_WithDefaultParameters_ShouldReturnAllBreeds()
    {
        // act
        var results = await breedsRepository.SearchAsync();

        // assert
        results.Should().HaveCountGreaterOrEqualTo(3);
    }

    [Theory]
    [MemberData(nameof(SeededBreeds))]
    public async Task GetByIdAsync_WithKnownId_ShouldReturnBreed(string breedId, string expectedName)
    {
        // act
        var breed = await breedsRepository.GetByIdAsync(Guid.Parse(breedId));

        // assert
        breed.Should().BeOfType<Breed>().Which.Name.Should().Be(expectedName);
    }

    [Fact]
    public async Task GetByIdAsync_WithUnknownId_ShouldReturnNull()
    {
        // arrange
        var unknownId = Guid.Empty;

        // act
        var breed = await breedsRepository.GetByIdAsync(unknownId);

        // assert
        breed.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(SeededBreeds))]
    public async Task GetByNameAsync_WithKnownName_ShouldReturnBreed(string expectedId, string breedName)
    {
        // act
        var breed = await breedsRepository.GetByNameAsync(breedName);

        // assert
        breed.Should().BeOfType<Breed>().Which.Id.Should().Be(expectedId);
    }

    [Fact]
    public async Task AddAsync_WithValidBreed_ShouldPass()
    {
        // arrange
        var species = await speciesRepository.GetAsync("Dog");
        var breedToAdd = BreedFactory.Create(name: "Poodle", species: species);

        // act
        await breedsRepository.AddAsync(breedToAdd);
        var breed = await breedsRepository.GetByIdAsync(breedToAdd.Id);

        // assert
        breed.Should().BeOfType<Breed>().Which.Name.Should().Be(breedToAdd.Name);
    }

    [Fact]
    public async Task UpdateAsync_WithValidBreed_ShouldPass()
    {
        // arrange
        var species = await speciesRepository.GetAsync("Dog");
        var breedToAdd = BreedFactory.Create(name: "Border-Collie", species: species);
        await breedsRepository.AddAsync(breedToAdd);

        // act
        var breedToUpdate = await breedsRepository.GetByIdAsync(breedToAdd.Id);
        breedToUpdate!.Name = "Border Collie";
        await breedsRepository.SaveChangesAsync();
        var breedToVerify = await breedsRepository.GetByIdAsync(breedToAdd.Id);

        // assert
        breedToVerify.Should().BeOfType<Breed>().Which.Name.Should().Be("Border Collie");
    }

    [Fact]
    public async Task DeleteAsync_WithKnownId_ShouldPass()
    {
        // arrange
        var species = await speciesRepository.GetAsync("Dog");
        var breedToDelete = BreedFactory.Create(name: "Pug", species: species);
        await breedsRepository.AddAsync(breedToDelete);

        // act
        await breedsRepository.DeleteAsync(breedToDelete);
        var breed = await breedsRepository.GetByIdAsync(breedToDelete.Id);

        // assert
        breed.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WithUnknownId_ShouldThrowException()
    {
        // arrange
        var unknownBreed = BreedFactory.Create();

        // act
        var act = async () => await breedsRepository.DeleteAsync(unknownBreed);

        // assert
        await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
    }

    public static readonly TheoryData<string, string> SeededBreeds = new ()
    {
        { SeedData.Breeds["Labrador Retriever"].Id.ToString(), "Labrador Retriever" },
        { SeedData.Breeds["German Shepherd"].Id.ToString(), "German Shepherd" },
        { SeedData.Breeds["Golden Retriever"].Id.ToString(), "Golden Retriever" }
    };
}
