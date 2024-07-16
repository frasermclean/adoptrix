using System.Net;
using Adoptrix.Api.Endpoints.Breeds;
using Adoptrix.Core.Contracts.Requests.Breeds;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

public class SearchBreedsEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.Client;

    [Theory, AdoptrixAutoData]
    public async Task SearchBreeds_WithValidRequest_ShouldReturnOk(SearchBreedsRequest request,
        List<BreedMatch> matchesToReturn)
    {
        // arrange
        fixture.BreedsRepositoryMock
            .Setup(repository =>
                repository.SearchAsync(It.IsAny<SearchBreedsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(matchesToReturn);

        // act
        var testResult =
            await httpClient.GETAsync<SearchBreedsEndpoint, SearchBreedsRequest, List<BreedMatch>>(request);

        // assert
        testResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        testResult.Result.Should().BeEquivalentTo(matchesToReturn);
    }
}
