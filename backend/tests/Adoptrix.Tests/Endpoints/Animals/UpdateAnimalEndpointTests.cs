using System.Net;
using Adoptrix.Core;
using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Endpoints.Animals;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Tests.Endpoints.Animals;

public class UpdateAnimalEndpointTests(App app) : TestBase<App>
{
    private readonly HttpClient httpClient = app.BasicAuthClient;

    [Theory, AdoptrixAutoData]
    public async Task UpdateAnimal_WithValidRequest_ShouldReturnOk(UpdateAnimalRequest request)
    {
        // act
        var (message, response) =
            await httpClient.PUTAsync<UpdateAnimalEndpoint, UpdateAnimalRequest, AnimalResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Id.Should().Be(request.AnimalId);
    }

    [Theory, AdoptrixAutoData]
    public async Task UpdateAnimal_WithInvalidAnimalId_ShouldReturnBadRequest(UpdateAnimalRequest request)
    {
        // arrange
        app.AnimalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Animal);

        // act
        var (message, response) =
            await httpClient.PUTAsync<UpdateAnimalEndpoint, UpdateAnimalRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("animalId");
    }

    [Theory, AdoptrixAutoData]
    public async Task UpdateAnimal_WithInvalidBreedId_ShouldReturnBadRequest(UpdateAnimalRequest request)
    {
        // arrange
        app.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var (message, response) =
            await httpClient.PUTAsync<UpdateAnimalEndpoint, UpdateAnimalRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("breedId");
    }
}
