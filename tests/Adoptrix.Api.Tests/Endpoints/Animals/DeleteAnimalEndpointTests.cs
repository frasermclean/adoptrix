using System.Net;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Initializer;

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
        var animalId = SeedData.Animals[3].Id;

        // act
        var message = await httpClient.DeleteAsync($"api/animals/{animalId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteAnimal_WithInvalidAnimalId_ShouldReturnNotFound()
    {
        // arrange
        var animalId = Guid.Empty;

        // act
        var message = await httpClient.DeleteAsync($"api/animals/{animalId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
