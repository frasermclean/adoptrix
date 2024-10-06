using System.Net;
using Adoptrix.Api.Endpoints.Species;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Core.Responses;
using Gridify;

namespace Adoptrix.Api.Tests.Endpoints.Species;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class SearchSpeciesEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{

    [Fact]
    public async Task SearchSpecies_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var query = new GridifyQuery
        {
            Filter = "animalCount>0",
            OrderBy = "name"
        };

        // act
        var (message, paging) =
            await fixture.Client.GETAsync<SearchSpeciesEndpoint, GridifyQuery, Paging<SpeciesMatch>>(query);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        paging.Count.Should().BePositive();
        paging.Data.Should().NotBeEmpty()
            .And.OnlyContain(match => match.AnimalCount > 0)
            .And.BeInAscendingOrder(match => match.Name);
    }
}
