using System.Net;
using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Endpoints.Animals;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Tests.Endpoints.Animals;

public class SearchAnimalEndpointTests(App app) : TestBase<App>
{
    private readonly HttpClient httpClient = app.Client;

    [Theory, AdoptrixAutoData]
    public async Task SearchAnimals_WithValidRequest_ShouldReturnOk(SearchAnimalsRequest request,
        List<AnimalMatch> matchesToReturn)
    {
        // arrange
        app.AnimalsRepositoryMock
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
