using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using Adoptrix.Database.Tests.Fixtures;

namespace Adoptrix.Database.Tests.Services;

[Trait("Category", "Integration")]
[Collection(nameof(DatabaseCollection))]
public class BreedsRepositoryTests(DatabaseFixture fixture)
{
    private readonly IBreedsRepository repository = fixture.BreedsRepository!;

    [Fact]
    public async Task SearchAsync_With_NoParameters_Returns_AllBreeds()
    {
        // act
        var results = await repository.SearchAsync();

        // assert
        results.Should().HaveCountGreaterOrEqualTo(3);
    }

    [Theory]
    [MemberData(nameof(GetKnownBreeds))]
    public async Task GetByIdAsync_WithKnownId_ShouldReturnExpectedResult(Guid breedId, string expectedName)
    {
        // act
        var result = await repository.GetByIdAsync(breedId);

        // assert
        result.Should().BeSuccess().Which.Value.Name.Should().Be(expectedName);
    }

    [Fact]
    public async Task GetByIdAsync_WithUnknownId_ShouldReturnFailure()
    {
        // arrange
        var unknownId = Guid.Empty;

        // act
        var result = await repository.GetByIdAsync(unknownId);

        // assert
        result.Should().BeFailure().Which.HasError<BreedNotFoundError>();
    }

    [Theory]
    [MemberData(nameof(GetKnownBreeds))]
    public async Task GetByNameAsync_WithKnownName_ShouldReturnExpectedResult(Guid expectedId, string breedName)
    {
        // act
        var result = await repository.GetByNameAsync(breedName);

        // assert
        result.Should().BeSuccess().Which.Value.Id.Should().Be(expectedId);
    }

    public static TheoryData<Guid, string> GetKnownBreeds() => new()
    {
        { BreedIds.LabradorRetriever, "Labrador Retriever" },
        { BreedIds.GermanShepherd, "German Shepherd" },
        { BreedIds.GoldenRetriever, "Golden Retriever" }
    };
}
