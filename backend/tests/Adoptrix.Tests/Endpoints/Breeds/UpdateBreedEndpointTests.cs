using System.Net;
using Adoptrix.Core;
using Adoptrix.Core.Contracts.Requests.Breeds;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Endpoints.Breeds;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Tests.Endpoints.Breeds;

public class UpdateBreedEndpointTests(App app) : TestBase<App>
{
    private readonly HttpClient httpClient = app.BasicAuthClient;

    [Theory, AdoptrixAutoData]
    public async Task UpdateBreed_WithValidRequest_ShouldReturnOk(UpdateBreedRequest request, Breed breed,
        Species species)
    {
        // arrange
        app.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(breed);
        app.SpeciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(species);

        // act
        var (message, response) =
            await httpClient.PUTAsync<UpdateBreedEndpoint, UpdateBreedRequest, BreedResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Id.Should().Be(breed.Id);
        app.BreedsRepositoryMock.Verify(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, AdoptrixAutoData]
    public async Task UpdateBreed_WithInvalidBreedId_ShouldReturnNotFound(UpdateBreedRequest request)
    {
        // arrange
        app.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var message = await httpClient.PUTAsync<UpdateBreedEndpoint, UpdateBreedRequest>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    [Theory, AdoptrixAutoData]
    public async Task UpdateBreed_WithInvalidSpeciesId_ShouldReturnBadRequest(UpdateBreedRequest request, Breed breed)
    {
        // arrange
        app.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(breed);
        app.SpeciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Species);

        // act
        var (message, response) = await httpClient.PUTAsync<UpdateBreedEndpoint, UpdateBreedRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("speciesId");
    }
}
