using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Contracts.Requests;
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
            BreedId = Guid.NewGuid(),
            Sex = "Female"
        };
        fixture.AnimalsRepositoryMock
            .Setup(repository => repository.SearchAsync(It.IsAny<string?>(), It.IsAny<Guid?>(), It.IsAny<Guid?>(),
                It.IsAny<Sex?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);

        // act
        var testResult =
            await httpClient.GETAsync<SearchAnimalsEndpoint, SearchAnimalsRequest, List<SearchAnimalsItem>>(request);

        // assert
        testResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        testResult.Result.Should().BeEquivalentTo(items);
    }
}
