using System.Net;
using Adoptrix.Api.Endpoints.Species;
using Adoptrix.Contracts.Requests;
using Adoptrix.Persistence.Responses;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Species;

public class SearchSpeciesEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.Client;

    [Theory, AdoptrixAutoData]
    public async Task SearchSpecies_WithValidRequest_ShouldReturnOk(SearchSpeciesRequest request,
        List<SearchSpeciesItem> matchesToReturn)
    {
        // arrange
        fixture.SpeciesRepositoryMock
            .Setup(repository => repository.SearchAsync(It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(matchesToReturn);

        // act
        var testResult =
            await httpClient.GETAsync<SearchSpeciesEndpoint, SearchSpeciesRequest, List<SearchSpeciesItem>>(request);

        // assert
        testResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        testResult.Result.Should().BeEquivalentTo(matchesToReturn);
    }
}
