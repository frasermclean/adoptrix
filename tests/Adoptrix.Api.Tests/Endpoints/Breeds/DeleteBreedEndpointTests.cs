using System.Net;
using Adoptrix.Api.Endpoints.Breeds;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

[Collection(nameof(ApiCollection))]
[Trait("Category", "Integration")]
public class DeleteBreedEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.AdminClient;

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
