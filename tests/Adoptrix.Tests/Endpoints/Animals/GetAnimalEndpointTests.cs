using System.Net;
using Adoptrix.Core;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Endpoints.Animals;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Tests.Endpoints.Animals;

public class GetAnimalEndpointTests(App app) : TestBase<App>
{
    [Theory, AdoptrixAutoData]
    public async Task GetAnimal_WithKnownAnimalId_ShouldReturnOk(GetAnimalRequest request, Animal animal)
    {
        // arrange
        app.AnimalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(animal);

        // act
        var (message, response) =
            await app.Client.GETAsync<GetAnimalEndpoint, GetAnimalRequest, AnimalResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Id.Should().Be(animal.Id);
    }

    [Theory, AdoptrixAutoData]
    public async Task GetAnimal_WithUnknownAnimalId_ShouldReturnNotFound(GetAnimalRequest request)
    {
        // arrange
        app.AnimalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Animal);

        // act
        var testResult = await app.Client.GETAsync<GetAnimalEndpoint, GetAnimalRequest, AnimalResponse>(request);

        // assert
        testResult.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
