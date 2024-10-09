using System.Net;
using Adoptrix.Api.Endpoints.Breeds;
using Adoptrix.Api.Tests.Fixtures;
using Gridify;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class SearchBreedsEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    [Fact]
    public async Task SearchBreeds_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var query = new GridifyQuery
        {
            Filter = "speciesName=Dog,animalCount>0",
            OrderBy = "animalCount desc",
        };

        // act
        var (message, paging) =
            await fixture.Client.GETAsync<SearchBreedsEndpoint, GridifyQuery, Paging<BreedResponse>>(query);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        paging.Count.Should().BePositive();
        paging.Data.Should().NotBeEmpty().And.BeInDescendingOrder(response => response.AnimalCount);
    }
}
