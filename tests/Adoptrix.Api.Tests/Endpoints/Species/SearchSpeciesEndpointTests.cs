using System.Net;
using Adoptrix.Api.Endpoints.Species;
using Adoptrix.Core.Contracts.Requests.Species;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Species;

public class SearchSpeciesEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.Client;

    [Theory, AdoptrixAutoData]
    public async Task SearchSpecies_WithValidRequest_ShouldReturnOk(SearchSpeciesRequest request,
        List<SpeciesMatch> matchesToReturn)
    {
        // arrange
        fixture.SpeciesRepositoryMock
            .Setup(repository =>
                repository.SearchAsync(It.IsAny<SearchSpeciesRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(matchesToReturn);

        // act
        var testResult =
            await httpClient.GETAsync<SearchSpeciesEndpoint, SearchSpeciesRequest, List<SpeciesMatch>>(request);

        // assert
        testResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        testResult.Result.Should().BeEquivalentTo(matchesToReturn);
    }
}
