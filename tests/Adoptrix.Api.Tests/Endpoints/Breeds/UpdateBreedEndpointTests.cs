using System.Net;
using Adoptrix.Api.Endpoints.Breeds;
using Adoptrix.Core;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

public class UpdateBreedEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.BasicAuthClient;

    [Theory, AdoptrixAutoData]
    public async Task UpdateBreed_WithValidRequest_ShouldReturnOk(Breed breed, Core.Species species)
    {
        // arrange
        var request = CreateRequest(breed.Id, species.Id);
        fixture.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(breed);
        fixture.SpeciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(species);

        // act
        var (message, response) =
            await httpClient.PUTAsync<UpdateBreedEndpoint, UpdateBreedRequest, BreedResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Id.Should().Be(breed.Id);
        fixture.BreedsRepositoryMock.Verify(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateBreed_WithInvalidBreedId_ShouldReturnNotFound()
    {
        // arrange
        var request = CreateRequest();
        fixture.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var message = await httpClient.PUTAsync<UpdateBreedEndpoint, UpdateBreedRequest>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    [Theory, AdoptrixAutoData]
    public async Task UpdateBreed_WithInvalidSpeciesId_ShouldReturnBadRequest(Breed breed)
    {
        // arrange
        var request = CreateRequest(breed.Id);
        fixture.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(breed);
        fixture.SpeciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Core.Species);

        // act
        var (message, response) = await httpClient.PUTAsync<UpdateBreedEndpoint, UpdateBreedRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("speciesId");
    }

    private static UpdateBreedRequest CreateRequest(Guid? breedId = null, Guid? speciesId = null) => new()
    {
        Name = "Sausage Dog",
        BreedId = breedId ?? Guid.NewGuid(),
        SpeciesId = speciesId ?? Guid.NewGuid(),
    };
}
