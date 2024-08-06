using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class SearchAnimalEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
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
