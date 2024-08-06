using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

[Collection(nameof(ApiCollection))]
[Trait("Category", "Integration")]
public class UpdateAnimalEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.AdminClient;

    [Fact]
    public async Task UpdateAnimal_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var request = CreateRequest(3, "Timmy", "Timmy is awesome", 2);

        // act
        var (message, response) =
            await httpClient.PUTAsync<UpdateAnimalEndpoint, UpdateAnimalRequest, AnimalResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Id.Should().Be(3);
        response.Name.Should().Be("Timmy");
        response.Description.Should().Be("Timmy is awesome");
        response.BreedId.Should().Be(2);
    }

    [Fact]
    public async Task UpdateAnimal_WithInvalidAnimalId_ShouldReturnNotFound()
    {
        // arrange
        var request = CreateRequest(-1);

        // act
        var message = await httpClient.PUTAsync<UpdateAnimalEndpoint, UpdateAnimalRequest>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateAnimal_WithInvalidBreedId_ShouldReturnBadRequest()
    {
        // arrange
        var request = CreateRequest(breedId: -1);

        // act
        var (message, response) =
            await httpClient.PUTAsync<UpdateAnimalEndpoint, UpdateAnimalRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("breedId");
    }

    private static UpdateAnimalRequest CreateRequest(int animalId = 1, string name = "Bobby",
        string? description = null,int breedId = 1, Sex sex = Sex.Male) => new()
    {
        AnimalId = animalId,
        Name = name,
        Description = description,
        BreedId = breedId,
        Sex = sex,
        DateOfBirth = new DateOnly(2022, 1, 3)
    };
}
