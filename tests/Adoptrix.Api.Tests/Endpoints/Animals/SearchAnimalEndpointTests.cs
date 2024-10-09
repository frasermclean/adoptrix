using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Api.Tests.Fixtures;
using Gridify;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class SearchAnimalEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    [Fact]
    public async Task SearchAnimals_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        const int pageSize = 3;
        var query = new GridifyQuery
        {
            Page = 1,
            PageSize = pageSize
        };

        // act
        var (message, paging) =
            await fixture.Client.GETAsync<SearchAnimalsEndpoint, GridifyQuery, Paging<SearchAnimalsItem>>(query);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        paging.Count.Should().BePositive();
        paging.Data.Should().HaveCountLessThanOrEqualTo(pageSize).And.AllSatisfy(item =>
        {
            item.Id.Should().NotBeEmpty();
            item.Slug.Should().NotBeNullOrWhiteSpace();
            item.Name.Should().NotBeNullOrWhiteSpace();
            item.SpeciesName.Should().NotBeNullOrWhiteSpace();
            item.BreedName.Should().NotBeNullOrWhiteSpace();
            item.Sex.Should().BeDefined();
            item.DateOfBirth.Should().NotBe(default);
            item.LastModifiedUtc.Should().NotBe(default);
            item.PreviewImageUrl.Should().BeNull();
        });
    }
}
