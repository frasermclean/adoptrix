using System.Net;
using Adoptrix.Api.Endpoints.Breeds;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Persistence;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class UpdateBreedEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    [Fact]
    public async Task UpdateBreed_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var request = CreateRequest("African-Grey Parrot");

        // act
        var (message, response) =
            await fixture.AdminClient.PUTAsync<UpdateBreedEndpoint, UpdateBreedRequest, BreedResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Id.Should().Be(request.BreedId);
        response.Name.Should().Be(request.Name);
        response.SpeciesName.Should().Be(request.SpeciesName);
        response.LastModifiedUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task UpdateBreed_WithInvalidBreedId_ShouldReturnNotFound()
    {
        // arrange
        var request = CreateRequest(breedId: -1);

        // act
        var message = await fixture.AdminClient.PUTAsync<UpdateBreedEndpoint, UpdateBreedRequest>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateBreed_WithInvalidSpeciesName_ShouldReturnBadRequest()
    {
        // arrange
        var request = CreateRequest(speciesName: "Spaghetti Monster");

        // act
        var (message, response) =
            await fixture.AdminClient.PUTAsync<UpdateBreedEndpoint, UpdateBreedRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("speciesName");
    }

    [Fact]
    public async Task UpdateBreed_WithExistingBreed_ShouldReturnConflict()
    {
        // arrange
        var request = CreateRequest("German Shepherd", 1);

        // act
        var (message, response) =
            await fixture.AdminClient.PUTAsync<UpdateBreedEndpoint, UpdateBreedRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.Conflict);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("name");
    }

    private static UpdateBreedRequest CreateRequest(string name = "African Grey Parrot",
        int breedId = SeedData.Breeds.AfricanGreyParrot, string speciesName = "Bird") => new()
    {
        Name = name,
        BreedId = breedId,
        SpeciesName = speciesName
    };
}
