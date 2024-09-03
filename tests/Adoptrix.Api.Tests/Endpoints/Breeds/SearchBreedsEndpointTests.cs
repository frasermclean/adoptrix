using System.Net;
using Adoptrix.Api.Endpoints.Breeds;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class SearchBreedsEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    [Fact]
    public async Task SearchBreeds_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var request = new SearchBreedsRequest();

        // act
        var (message, matches) =
            await fixture.Client.GETAsync<SearchBreedsEndpoint, SearchBreedsRequest, List<BreedMatch>>(request);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        matches.Should().NotBeEmpty();
    }
}
