using System.Net;
using Adoptrix.Api.Tests.Fixtures;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class DeleteAnimalEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    private readonly HttpClient httpClient = fixture.CreateClient();

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
