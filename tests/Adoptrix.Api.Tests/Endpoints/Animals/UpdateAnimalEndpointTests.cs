using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Core;
using Adoptrix.Initializer;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class UpdateAnimalEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    [Fact]
    public async Task UpdateAnimal_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var request = CreateRequest(SeedData.Animals[0].Id, "Timmy", "Timmy is awesome", "German Shepherd");

        // act
        var (message, response) =
            await fixture.AdminClient.PUTAsync<UpdateAnimalEndpoint, UpdateAnimalRequest, AnimalResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Name.Should().Be("Timmy");
        response.Description.Should().Be("Timmy is awesome");
    }

    [Fact]
    public async Task UpdateAnimal_WithInvalidAnimalId_ShouldReturnNotFound()
    {
        // arrange
        var request = CreateRequest(Guid.Empty);

        // act
        var message = await fixture.AdminClient.PUTAsync<UpdateAnimalEndpoint, UpdateAnimalRequest>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateAnimal_WithInvalidBreedName_ShouldReturnBadRequest()
    {
        // arrange
        var request = CreateRequest(SeedData.Animals[0].Id, breedName: "Flower");

        // act
        var (message, response) =
            await fixture.AdminClient.PUTAsync<UpdateAnimalEndpoint, UpdateAnimalRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("breedName");
    }

    private static UpdateAnimalRequest CreateRequest(Guid? animalId = null, string name = "Bobby",
        string? description = null, string? breedName = null, Sex sex = Sex.Male) => new()
    {
        AnimalId = animalId ?? Guid.NewGuid(),
        Name = name,
        Description = description,
        BreedName = breedName ?? "Golden Retriever",
        Sex = sex,
        DateOfBirth = new DateOnly(2022, 1, 3)
    };
}
