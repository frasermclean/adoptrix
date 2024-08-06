using System.Net;
using Adoptrix.Api.Endpoints.Species;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Contracts.Responses;

namespace Adoptrix.Api.Tests.Endpoints.Species;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class GetSpeciesEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    private readonly HttpClient httpClient = fixture.Client;

    [Fact]
    public async Task GetSpecies_WithKnownSpeciesName_ShouldReturnOk()
    {
        // arrange
        var request = new GetSpeciesRequest("Dog");

        // act
        var (message, response) = await httpClient.GETAsync<GetSpeciesEndpoint, GetSpeciesRequest, SpeciesResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Name.Should().Be("Dog");
    }

    [Fact]
    public async Task GetSpecies_WithUnknownSpeciesName_ShouldReturnNotFound()
    {
        // arrange
        var request = new GetSpeciesRequest("Unknown");

        // act
        var message = await httpClient.GETAsync<GetSpeciesEndpoint, GetSpeciesRequest>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
