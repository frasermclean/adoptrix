using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

public class AddAnimalEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    [Theory, AdoptrixAutoData]
    public async Task AddAnimal_WithValidRequest_ShouldReturnCreated(Breed breed)
    {
        // arrange
        var request = CreateRequest();
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
        response.DateOfBirth.Should().Be(request.DateOfBirth);
        response.Slug.Should().StartWith("buddy-");
        response.Age.Should().NotBeEmpty();
    }

    [Fact]
    public async Task AddAnimal_WithInvalidBreedId_ShouldReturnBadRequest()
    {
        // arrange
        var request = CreateRequest();
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
    public async Task AddAnimal_WithInvalidRole_ShouldReturnForbidden(AddAnimalRequest request, Breed breed)
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

    private static AddAnimalRequest CreateRequest() => new()
    {
        Name = "Buddy",
        Description = null,
        BreedId = 4,
        Sex = Sex.Male,
        DateOfBirth = new DateOnly(2019, 1, 1),
        UserId = Guid.NewGuid()
    };
}
