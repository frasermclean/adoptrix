using System.Net;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Initializer;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class DeleteAnimalEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    [Fact]
    public async Task DeleteAnimal_WithValidRequest_ShouldReturnNoContent()
    {
        // arrange
        var animalId = SeedData.Percy.Id;

        // act
        var message = await fixture.AdminClient.DeleteAsync($"/api/animals/{animalId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteAnimal_WithInvalidAnimalId_ShouldReturnNotFound()
    {
        // arrange
        var animalId = Guid.Empty;

        // act
        var message = await fixture.AdminClient.DeleteAsync($"/api/animals/{animalId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
