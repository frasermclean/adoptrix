using System.Net;
using Adoptrix.Api.Tests.Fixtures;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class DeleteBreedEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    private readonly HttpClient httpClient = fixture.CreateClient();

    [Fact]
    public async Task DeleteBreed_WithValidRequest_ShouldReturnNoContent()
    {
        // arrange
        const int breedId = 3;

        // act
        var message = await httpClient.DeleteAsync($"/api/breeds/{breedId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteBreed_WithInvalidAnimalId_ShouldReturnNotFound()
    {
        // arrange
        const int breedId = -1;

        // act
        var message = await httpClient.DeleteAsync($"/api/breeds/{breedId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
