using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Core;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

public class GetAnimalEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    [Theory, AdoptrixAutoData]
    public async Task GetAnimal_WithKnownAnimalId_ShouldReturnOk(GetAnimalRequest request, Animal animal)
    {
        // arrange
        fixture.AnimalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(animal);

        // act
        var (message, response) =
            await fixture.Client.GETAsync<GetAnimalEndpoint, GetAnimalRequest, AnimalResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Id.Should().Be(animal.Id);
        response.Name.Should().Be(animal.Name);
        response.Description.Should().Be(animal.Description);
        response.SpeciesId.Should().Be(animal.Breed.Species.Id);
        response.SpeciesName.Should().Be(animal.Breed.Species.Name);
        response.BreedId.Should().Be(animal.Breed.Id);
        response.BreedName.Should().Be(animal.Breed.Name);
        response.Sex.Should().Be(animal.Sex);
        response.DateOfBirth.Should().Be(animal.DateOfBirth);
        response.Age.Should().NotBeEmpty();
    }

    [Theory, AdoptrixAutoData]
    public async Task GetAnimal_WithUnknownAnimalId_ShouldReturnNotFound(GetAnimalRequest request)
    {
        // arrange
        fixture.AnimalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Animal);

        // act
        var testResult = await fixture.Client.GETAsync<GetAnimalEndpoint, GetAnimalRequest, AnimalResponse>(request);

        // assert
        testResult.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
