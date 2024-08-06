using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Persistence.Responses;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

[Collection(nameof(ApiCollection))]
[Trait("Category", "Integration")]
public class SearchAnimalEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.Client;

    [Fact]
    public async Task SearchAnimals_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var request = new SearchAnimalsRequest();

        // act
        var (message, matches) =
            await httpClient.GETAsync<SearchAnimalsEndpoint, SearchAnimalsRequest, List<AnimalMatch>>(request);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        matches.Should().NotBeEmpty();
    }
}
