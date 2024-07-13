using System.Net;
using Adoptrix.Core;
using Adoptrix.Core.Events;
using Adoptrix.Endpoints.Animals;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Tests.Endpoints.Animals;

public class DeleteAnimalEndpointTests(App app) : TestBase<App>
{
    private readonly HttpClient httpClient = app.BasicAuthClient;

    [Theory, AdoptrixAutoData]
    public async Task DeleteAnimal_WithValidRequest_ShouldReturnNoContent(DeleteAnimalRequest request, Animal animal)
    {
        // arrange
        app.AnimalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(animal);

        // act
        var message = await httpClient.DELETEAsync<DeleteAnimalEndpoint, DeleteAnimalRequest>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NoContent);
        app.EventPublisherMock.Verify(
            publisher => publisher.PublishAsync(It.IsAny<AnimalDeletedEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory, AdoptrixAutoData]
    public async Task DeleteAnimal_WithInvalidAnimalId_ShouldReturnNotFound(DeleteAnimalRequest request)
    {
        // arrange
        app.AnimalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Animal);

        // act
        var message = await httpClient.DELETEAsync<DeleteAnimalEndpoint, DeleteAnimalRequest>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
