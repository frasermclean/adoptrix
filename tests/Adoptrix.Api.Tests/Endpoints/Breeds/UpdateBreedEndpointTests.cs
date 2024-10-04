using System.Net;
using Adoptrix.Api.Endpoints.Breeds;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class UpdateBreedEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    [Fact]
    public async Task UpdateBreed_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var request = CreateRequest(speciesName: "Bird");

        // act
        var (message, response) =
            await fixture.AdminClient.PUTAsync<UpdateBreedEndpoint, UpdateBreedRequest, BreedResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Id.Should().Be(5);
        response.Name.Should().Be("Budgerigar");
        response.SpeciesName.Should().Be("Bird");
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
        var (message, response) = await fixture.AdminClient.PUTAsync<UpdateBreedEndpoint, UpdateBreedRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.Conflict);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("name");
    }

    private static UpdateBreedRequest CreateRequest(string name = "Budgerigar", int breedId = 5, string speciesName = "Dog") => new()
    {
        Name = name,
        BreedId = breedId,
        SpeciesName = speciesName,
        UserId = Guid.NewGuid()
    };
}
