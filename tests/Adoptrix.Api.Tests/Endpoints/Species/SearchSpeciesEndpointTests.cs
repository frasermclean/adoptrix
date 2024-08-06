using System.Net;
using Adoptrix.Api.Endpoints.Species;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;

namespace Adoptrix.Api.Tests.Endpoints.Species;

[Collection(nameof(ApiCollection))]
[Trait("Category", "Integration")]
public class SearchSpeciesEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.Client;

    [Fact]
    public async Task SearchSpecies_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var request = new SearchSpeciesRequest
        {
            WithAnimals = true
        };

        // act
        var (message, matches) =
            await httpClient.GETAsync<SearchSpeciesEndpoint, SearchSpeciesRequest, List<SpeciesMatch>>(request);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        matches.Should().HaveCountGreaterThan(0);
    }
}
