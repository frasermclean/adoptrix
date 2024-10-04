using System.Net;
using Adoptrix.Api.Endpoints.Species;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;

namespace Adoptrix.Api.Tests.Endpoints.Species;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class SearchSpeciesEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{

    [Fact]
    public async Task SearchSpecies_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var request = new SearchSpeciesRequest
        {
            WithAnimals = true
        };

        // act
        var (message, matches) =
            await fixture.Client.GETAsync<SearchSpeciesEndpoint, SearchSpeciesRequest, List<SpeciesMatch>>(request);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        matches.Should().HaveCountGreaterThan(0);
    }
}
