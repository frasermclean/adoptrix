using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Core;
using Adoptrix.Core.Events;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

public class DeleteAnimalEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.BasicAuthClient;

    [Theory, AdoptrixAutoData]
    public async Task DeleteAnimal_WithValidRequest_ShouldReturnNoContent(DeleteAnimalRequest request, Animal animal)
    {
        // arrange
        fixture.AnimalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(animal);

        // act
        var message = await httpClient.DELETEAsync<DeleteAnimalEndpoint, DeleteAnimalRequest>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NoContent);
        fixture.EventPublisherMock.Verify(
            publisher => publisher.PublishAsync(It.IsAny<AnimalDeletedEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory, AdoptrixAutoData]
    public async Task DeleteAnimal_WithInvalidAnimalId_ShouldReturnNotFound(DeleteAnimalRequest request)
    {
        // arrange
        fixture.AnimalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Animal);

        // act
        var message = await httpClient.DELETEAsync<DeleteAnimalEndpoint, DeleteAnimalRequest>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
