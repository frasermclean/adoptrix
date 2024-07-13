using System.Net;
using Adoptrix.Core;
using Adoptrix.Endpoints.Breeds;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Tests.Endpoints.Breeds;

public class DeleteBreedEndpointTests(App app) : TestBase<App>
{
    private readonly HttpClient httpClient = app.BasicAuthClient;

    [Theory, AdoptrixAutoData]
    public async Task DeleteBreed_WithValidRequest_ShouldReturnNoContent(DeleteBreedRequest request, Breed breed)
    {
        // arrange
        app.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(breed);

        // act
        var message = await httpClient.DELETEAsync<DeleteBreedEndpoint, DeleteBreedRequest>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NoContent);
    }

    [Theory, AdoptrixAutoData]
    public async Task DeleteBreed_WithInvalidAnimalId_ShouldReturnNotFound(DeleteBreedRequest request)
    {
        // arrange
        app.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var message = await httpClient.DELETEAsync<DeleteBreedEndpoint, DeleteBreedRequest>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
