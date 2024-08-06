using System.Net;
using System.Net.Http.Json;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Contracts.Responses;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class GetAnimalEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    [Fact]
    public async Task GetAnimal_WithKnownAnimalSlug_ShouldReturnOk()
    {
        // arrange
        const string animalSlug = "alberto-2024-02-14";

        // act
        var message = await fixture.Client.GetAsync($"api/animals/{animalSlug}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var response = await message.Content.ReadFromJsonAsync<AnimalResponse>();
        response!.Id.Should().Be(1);
        response.Name.Should().Be("Alberto");
        response.Description.Should().NotBeEmpty();
        response.SpeciesName.Should().Be("Dog");
        response.BreedId.Should().Be(1);
        response.BreedName.Should().Be("Labrador Retriever");
        response.Sex.Should().Be("Male");
        response.DateOfBirth.Should().Be(new DateOnly(2024, 2, 14));
        response.Age.Should().NotBeEmpty();
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
