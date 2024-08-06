using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Core;
using Adoptrix.Core.Events;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

[Collection(nameof(ApiCollection))]
[Trait("Category", "Integration")]
public class DeleteAnimalEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.AdminClient;

    [Fact]
    public async Task DeleteAnimal_WithValidRequest_ShouldReturnNoContent()
    {
        // arrange
        const int animalId = 4;

        // act
        var message = await httpClient.DeleteAsync($"api/animals/{animalId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteAnimal_WithInvalidAnimalId_ShouldReturnNotFound()
    {
        // arrange
        const int animalId = -1;

        // act
        var message = await httpClient.DeleteAsync($"api/animals/{animalId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
