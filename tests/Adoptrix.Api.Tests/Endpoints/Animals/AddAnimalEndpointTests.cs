using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

public class AddAnimalEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    [Theory, AdoptrixAutoData]
    public async Task AddAnimal_WithValidRequest_ShouldReturnCreated(AddAnimalRequest request, Breed breed)
    {
        // arrange
        fixture.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(breed);

        // act
        var (message, response) =
            await fixture.AdminClient.POSTAsync<AddAnimalEndpoint, AddAnimalRequest, AnimalResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.Created);
        message.Headers.Location.Should().NotBeNull();
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
            await fixture.AdminClient.POSTAsync<AddAnimalEndpoint, AddAnimalRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("breedId");
    }

    [Theory, AdoptrixAutoData]
    public async Task AddAnimal_InvalidRole_ShouldReturnForbidden(AddAnimalRequest request, Breed breed)
    {
        // arrange
        fixture.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(breed);

        // act
        var testResult =
            await fixture.UserClient.POSTAsync<AddAnimalEndpoint, AddAnimalRequest, AnimalResponse>(request);

        // assert
        testResult.Response.Should().HaveStatusCode(HttpStatusCode.Forbidden);
    }
}
