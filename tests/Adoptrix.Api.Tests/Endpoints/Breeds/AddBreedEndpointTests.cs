using System.Net;
using Adoptrix.Api.Endpoints.Breeds;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Contracts.Requests;
using Adoptrix.Core.Responses;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class AddBreedEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    [Fact]
    public async Task AddBreed_WithValidRequest_ShouldReturnCreated()
    {
        // arrange
        var request = CreateRequest("Bruno", "Dog");

        // act
        var (message, response) =
            await fixture.AdminClient.POSTAsync<AddBreedEndpoint, AddBreedRequest, BreedResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.Created);
        message.Headers.Location.Should().NotBeNull();
        response.Name.Should().Be("Bruno");
        response.SpeciesName.Should().Be("Dog");
    }

    [Fact]
    public async Task AddBreed_WithInvalidSpeciesName_ShouldReturnBadRequest()
    {
        // arrange
        var request = CreateRequest("It", "Spaghetti Monster");

        // act
        var (message, response) =
            await fixture.AdminClient.POSTAsync<AddBreedEndpoint, AddBreedRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("speciesName");
    }

    [Fact]
    public async Task AddBreed_WithExistingBreed_ShouldReturnConflict()
    {
        // arrange
        var request = CreateRequest();

        // act
        var (message, response) =
            await fixture.AdminClient.POSTAsync<AddBreedEndpoint, AddBreedRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.Conflict);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("name");
    }

    private static AddBreedRequest CreateRequest(string? name = null, string? speciesName = null) => new()
    {
        Name = name ?? "Golden Retriever",
        SpeciesName = speciesName ?? "Dog",
        UserId = Guid.NewGuid()
    };
}
