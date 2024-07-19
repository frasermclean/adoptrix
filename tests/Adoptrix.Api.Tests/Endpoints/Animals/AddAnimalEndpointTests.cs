using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Core;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

public class AddAnimalEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.BasicAuthClient;

    [Theory, AdoptrixAutoData]
    public async Task AddAnimal_WithValidRequest_ShouldReturnCreated(AddAnimalRequest request, Breed breed)
    {
        // arrange
        fixture.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(breed);

        // act
        var (message, response) =
            await httpClient.POSTAsync<AddAnimalEndpoint, AddAnimalRequest, AnimalResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.Created);
        message.Headers.Location.Should().NotBeNull();
        response.Id.Should().NotBeEmpty();
        response.Name.Should().Be(request.Name);
    }

    [Theory, AdoptrixAutoData]
    public async Task AddAnimal_WithInvalidBreedId_ShouldReturnBadRequest(AddAnimalRequest request)
    {
        // arrange
        fixture.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var (message, response) =
            await httpClient.POSTAsync<AddAnimalEndpoint, AddAnimalRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("breedId");
    }
}
