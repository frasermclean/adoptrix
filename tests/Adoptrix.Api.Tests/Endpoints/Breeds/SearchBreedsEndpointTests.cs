using System.Net;
using Adoptrix.Api.Endpoints.Breeds;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

[Collection(nameof(ApiCollection))]
[Trait("Category", "Integration")]
public class SearchBreedsEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.Client;

    [Fact]
    public async Task SearchBreeds_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var request = new SearchBreedsRequest();

        // act
        var (message, matches) =
            await httpClient.GETAsync<SearchBreedsEndpoint, SearchBreedsRequest, List<BreedMatch>>(request);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        matches.Should().NotBeEmpty();
    }
}
