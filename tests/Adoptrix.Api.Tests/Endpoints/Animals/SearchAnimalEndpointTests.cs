using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

public class SearchAnimalEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.Client;

    [Theory, AdoptrixAutoData]
    public async Task SearchAnimals_WithValidRequest_ShouldReturnOk(SearchAnimalsRequest request,
        List<AnimalMatch> matchesToReturn)
    {
        // arrange
        fixture.AnimalsRepositoryMock
            .Setup(repository =>
                repository.SearchAsync(It.IsAny<SearchAnimalsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(matchesToReturn);

        // act
        var testResult =
            await httpClient.GETAsync<SearchAnimalsEndpoint, SearchAnimalsRequest, List<AnimalMatch>>(request);

        // assert
        testResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        testResult.Result.Should().BeEquivalentTo(matchesToReturn);
    }
}
