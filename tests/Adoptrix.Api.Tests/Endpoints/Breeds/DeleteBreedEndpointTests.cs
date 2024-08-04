using System.Net;
using Adoptrix.Api.Endpoints.Breeds;
using Adoptrix.Core;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

public class DeleteBreedEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.AdminClient;

    [Theory, AdoptrixAutoData]
    public async Task DeleteBreed_WithValidRequest_ShouldReturnNoContent(DeleteBreedRequest request, Breed breed)
    {
        // arrange
        fixture.BreedsRepositoryMock
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
        fixture.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var message = await httpClient.DELETEAsync<DeleteBreedEndpoint, DeleteBreedRequest>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
