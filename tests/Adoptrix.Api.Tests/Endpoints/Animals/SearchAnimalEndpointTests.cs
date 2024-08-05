using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Persistence.Responses;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

public class SearchAnimalEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.Client;

    [Theory, AdoptrixAutoData]
    public async Task SearchAnimals_WithValidRequest_ShouldReturnOk(List<SearchAnimalsItem> items)
    {
        // arrange
        var request = new SearchAnimalsRequest
        {
            BreedId = 2,
            Sex = "Female"
        };
        fixture.AnimalsRepositoryMock
            .Setup(repository => repository.SearchAsync(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<string?>(),
                It.IsAny<Sex?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);

        // act
        var testResult =
            await httpClient.GETAsync<SearchAnimalsEndpoint, SearchAnimalsRequest, List<AnimalMatch>>(request);

        // assert
        testResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        testResult.Result.Should().HaveCount(items.Count);
    }
}
