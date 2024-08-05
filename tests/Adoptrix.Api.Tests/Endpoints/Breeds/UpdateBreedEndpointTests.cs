using System.Net;
using Adoptrix.Api.Endpoints.Breeds;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

public class UpdateBreedEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.AdminClient;

    [Theory, AdoptrixAutoData]
    public async Task UpdateBreed_WithValidRequest_ShouldReturnOk(Breed breed, Core.Species species)
    {
        // arrange
        var request = CreateRequest(breed.Id, species.Name);
        fixture.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(breed);
        fixture.SpeciesRepositoryMock
            .Setup(repository => repository.GetAsync(request.SpeciesName, It.IsAny<CancellationToken>()))
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
    public async Task UpdateBreed_WithInvalidSpeciesName_ShouldReturnBadRequest(Breed breed)
    {
        // arrange
        var request = CreateRequest(breed.Id);
        fixture.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(breed);
        fixture.SpeciesRepositoryMock
            .Setup(repository => repository.GetAsync(request.SpeciesName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Core.Species);

        // act
        var (message, response) = await httpClient.PUTAsync<UpdateBreedEndpoint, UpdateBreedRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("speciesName");
    }

    private static UpdateBreedRequest CreateRequest(int? breedId = null, string? speciesName = null) => new()
    {
        Name = "Sausage Dog",
        BreedId = breedId ?? Random.Shared.Next(),
        SpeciesName = speciesName ?? "Dog"
    };
}
