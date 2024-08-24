using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Initializer;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class UpdateAnimalEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    private readonly HttpClient httpClient = fixture.CreateClient();

    [Fact]
    public async Task UpdateAnimal_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var request = CreateRequest(SeedData.Animals[0].Id, "Timmy", "Timmy is awesome", 2);

        // act
        var (message, response) =
            await httpClient.PUTAsync<UpdateAnimalEndpoint, UpdateAnimalRequest, AnimalResponse>(request);

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
        var message = await httpClient.PUTAsync<UpdateAnimalEndpoint, UpdateAnimalRequest>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateAnimal_WithInvalidBreedId_ShouldReturnBadRequest()
    {
        // arrange
        var request = CreateRequest(SeedData.Animals[0].Id, breedId: -1);

        // act
        var (message, response) =
            await httpClient.PUTAsync<UpdateAnimalEndpoint, UpdateAnimalRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("breedId");
    }

    private static UpdateAnimalRequest CreateRequest(Guid? animalId = null, string name = "Bobby",
        string? description = null, int breedId = 1, Sex sex = Sex.Male) => new()
    {
        AnimalId = animalId ?? Guid.NewGuid(),
        Name = name,
        Description = description,
        BreedId = breedId,
        Sex = sex.ToString(),
        DateOfBirth = new DateOnly(2022, 1, 3)
    };
}
