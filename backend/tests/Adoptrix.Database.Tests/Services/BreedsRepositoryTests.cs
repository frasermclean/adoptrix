using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using Adoptrix.Database.Tests.Fixtures;
using Adoptrix.Domain.Models.Factories;

namespace Adoptrix.Database.Tests.Services;

[Trait("Category", "Integration")]
[Collection(nameof(DatabaseCollection))]
public class BreedsRepositoryTests(DatabaseFixture fixture)
{
    private readonly IBreedsRepository breedsRepository = fixture.BreedsRepository!;
    private readonly ISpeciesRepository speciesRepository = fixture.SpeciesRepository!;

    [Fact]
    public async Task SearchAsync_With_NoParameters_Returns_AllBreeds()
    {
        // act
        var results = await breedsRepository.SearchAsync();

        // assert
        results.Should().HaveCountGreaterOrEqualTo(3);
    }

    [Theory]
    [MemberData(nameof(GetKnownBreeds))]
    public async Task GetByIdAsync_WithKnownId_ShouldReturnExpectedResult(Guid breedId, string expectedName)
    {
        // act
        var result = await breedsRepository.GetByIdAsync(breedId);

        // assert
        result.Should().BeSuccess().Which.Value.Name.Should().Be(expectedName);
    }

    [Fact]
    public async Task GetByIdAsync_WithUnknownId_ShouldReturnFailure()
    {
        // arrange
        var unknownId = Guid.Empty;

        // act
        var result = await breedsRepository.GetByIdAsync(unknownId);

        // assert
        result.Should().BeFailure().Which.HasError<BreedNotFoundError>();
    }

    [Theory]
    [MemberData(nameof(GetKnownBreeds))]
    public async Task GetByNameAsync_WithKnownName_ShouldReturnExpectedResult(Guid expectedId, string breedName)
    {
        // act
        var result = await breedsRepository.GetByNameAsync(breedName);

        // assert
        result.Should().BeSuccess().Which.Value.Id.Should().Be(expectedId);
    }

    [Fact]
    public async Task AddAsync_WithValidBreed_ShouldReturnSuccess()
    {
        // arrange
        var speciesResult = await speciesRepository.GetByIdAsync(SpeciesIds.Dog);
        var breed = BreedFactory.Create(name: "Poodle", species: speciesResult.Value);

        // act
        var result = await breedsRepository.AddAsync(breed);

        // assert
        result.Should().BeSuccess();
    }

    [Fact]
    public async Task UpdateAsync_WithValidBreed_ShouldReturnSuccess()
    {
        // arrange
        var speciesResult = await speciesRepository.GetByIdAsync(SpeciesIds.Dog);
        var breed = BreedFactory.Create(name: "Kelpie", species: speciesResult.Value);
        await breedsRepository.AddAsync(breed);

        // act
        breed.Name = "Kelpie Cross";
        var result = await breedsRepository.UpdateAsync(breed);

        // assert
        result.Should().BeSuccess();
    }

    [Fact]
    public async Task DeleteAsync_WithKnownId_ShouldReturnSuccess()
    {
        // arrange
        var speciesResult = await speciesRepository.GetByIdAsync(SpeciesIds.Dog);
        var breed = BreedFactory.Create(name: "Pug", species: speciesResult.Value);
        await breedsRepository.AddAsync(breed);

        // act
        var deleteResult = await breedsRepository.DeleteAsync(breed.Id);
        var getResult = await breedsRepository.GetByIdAsync(breed.Id);

        // assert
        deleteResult.Should().BeSuccess();
        getResult.Should().BeFailure().Which.HasError<BreedNotFoundError>();
    }

    [Fact]
    public async Task DeleteAsync_WithUnknownId_ShouldReturnFailure()
    {
        // arrange
        var unknownId = Guid.Empty;

        // act
        var result = await breedsRepository.DeleteAsync(unknownId);

        // assert
        result.Should().BeFailure().Which.HasError<BreedNotFoundError>();
    }

    public static TheoryData<Guid, string> GetKnownBreeds() => new()
    {
        { BreedIds.LabradorRetriever, "Labrador Retriever" },
        { BreedIds.GermanShepherd, "German Shepherd" },
        { BreedIds.GoldenRetriever, "Golden Retriever" }
    };
}
