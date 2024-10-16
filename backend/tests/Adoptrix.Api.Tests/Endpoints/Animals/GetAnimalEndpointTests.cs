using System.Net;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Initializer;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class GetAnimalEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    [Fact]
    public async Task GetAnimal_WithKnownAnimalSlug_ShouldReturnOk()
    {
        // arrange
        var animalSlug = SeedData.Alberto.Slug;

        // act
        var message = await fixture.Client.GetAsync($"api/animals/{animalSlug}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAnimal_WithKnownAnimalId_ShouldReturnOk()
    {
        // arrange
        var animalId = SeedData.Alberto.Id;

        // act
        var message = await fixture.Client.GetAsync($"api/animals/{animalId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAnimal_WithUnknownAnimalId_ShouldReturnNotFound()
    {
        // arrange
        var animalId = Guid.Empty;

        // act
        var message = await fixture.Client.GetAsync($"api/animals/{animalId}");

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAnimal_WithUnknownAnimalSlug_ShouldReturnNotFound()
    {
        // arrange
        const string animalSlug = "unknown-animal";

        // act
        var message = await fixture.Client.GetAsync($"api/animals/{animalSlug}");

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
