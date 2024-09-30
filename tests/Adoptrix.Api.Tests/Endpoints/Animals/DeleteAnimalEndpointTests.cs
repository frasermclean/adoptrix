using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Core.Requests;
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
        var request = CreateRequest(SeedData.Animals[3].Id);

        // act
        var result = await fixture.AdminClient.DELETEAsync<DeleteAnimalEndpoint, DeleteAnimalRequest, string>(request);

        // assert
        result.Response.Should().HaveStatusCode(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteAnimal_WithInvalidAnimalId_ShouldReturnNotFound()
    {
        // arrange
        var request = CreateRequest(SeedData.Animals[3].Id);

        // act
        var result = await fixture.AdminClient.DELETEAsync<DeleteAnimalEndpoint, DeleteAnimalRequest, string>(request);

        // assert
        result.Response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    private static DeleteAnimalRequest CreateRequest(Guid? animalId = null, Guid? userId = null) => new()
    {
        AnimalId = animalId ?? Guid.NewGuid(),
        UserId = userId ?? Guid.NewGuid()
    };
}
