using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Core;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

public class UpdateAnimalEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.BasicAuthClient;

    [Theory, AdoptrixAutoData]
    public async Task UpdateAnimal_WithValidRequest_ShouldReturnOk(UpdateAnimalRequest request, Animal animal,
        Breed breed)
    {
        // arrange
        fixture.AnimalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(animal);
        fixture.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(breed);

        // act
        var (message, response) =
            await httpClient.PUTAsync<UpdateAnimalEndpoint, UpdateAnimalRequest, AnimalResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Id.Should().Be(animal.Id);
    }

    [Theory, AdoptrixAutoData]
    public async Task UpdateAnimal_WithInvalidAnimalId_ShouldReturnNotFound(UpdateAnimalRequest request)
    {
        // arrange
        fixture.AnimalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Animal);

        // act
        var message = await httpClient.PUTAsync<UpdateAnimalEndpoint, UpdateAnimalRequest>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    [Theory, AdoptrixAutoData]
    public async Task UpdateAnimal_WithInvalidBreedId_ShouldReturnBadRequest(UpdateAnimalRequest request, Animal animal)
    {
        // arrange
        fixture.AnimalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(animal);
        fixture.BreedsRepositoryMock
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
