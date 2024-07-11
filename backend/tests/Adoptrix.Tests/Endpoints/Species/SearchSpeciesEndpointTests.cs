using System.Net;
using Adoptrix.Core.Contracts.Requests.Species;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Endpoints.Species;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Tests.Endpoints.Species;

public class SearchSpeciesEndpointTests(App app) : TestBase<App>
{
    private readonly HttpClient httpClient = app.Client;

    [Theory, AdoptrixAutoData]
    public async Task SearchSpecies_WithValidRequest_ShouldReturnOk(SearchSpeciesRequest request,
        List<SpeciesMatch> matchesToReturn)
    {
        // arrange
        app.SpeciesRepositoryMock
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
