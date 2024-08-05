using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

public class GetAnimalEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    [Theory, AdoptrixAutoData]
    public async Task GetAnimal_WithKnownAnimalId_ShouldReturnOk(Animal animal)
    {
        // arrange
        const int animalId = 23;
        var request = new GetAnimalRequest(animalId.ToString());
        fixture.AnimalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(animalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(animal);

        // act
        var (message, response) =
            await fixture.Client.GETAsync<GetAnimalEndpoint, GetAnimalRequest, AnimalResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Id.Should().Be(animal.Id);
        response.Name.Should().Be(animal.Name);
        response.Description.Should().Be(animal.Description);
        response.SpeciesName.Should().Be(animal.Breed.Species.Name);
        response.BreedId.Should().Be(animal.Breed.Id);
        response.BreedName.Should().Be(animal.Breed.Name);
        response.Sex.Should().Be(animal.Sex.ToString());
        response.DateOfBirth.Should().Be(animal.DateOfBirth);
        response.Age.Should().NotBeEmpty();
        fixture.AnimalsRepositoryMock.Verify(
            repository => repository.GetByIdAsync(animalId, It.IsAny<CancellationToken>()), Times.Once);
    }
    [Theory, AdoptrixAutoData]
    public async Task GetAnimal_WithKnownAnimalSlug_ShouldReturnOk(Animal animal)
    {
        // arrange
        const string animalSlug = "bobby-2020-12-12";
        var request = new GetAnimalRequest(animalSlug);
        fixture.AnimalsRepositoryMock
            .Setup(repository => repository.GetBySlugAsync(animalSlug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(animal);

        // act
        var (message, response) =
            await fixture.Client.GETAsync<GetAnimalEndpoint, GetAnimalRequest, AnimalResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Id.Should().Be(animal.Id);
        response.Name.Should().Be(animal.Name);
        response.Description.Should().Be(animal.Description);
        response.SpeciesName.Should().Be(animal.Breed.Species.Name);
        response.BreedId.Should().Be(animal.Breed.Id);
        response.BreedName.Should().Be(animal.Breed.Name);
        response.Sex.Should().Be(animal.Sex.ToString());
        response.DateOfBirth.Should().Be(animal.DateOfBirth);
        response.Age.Should().NotBeEmpty();
        fixture.AnimalsRepositoryMock.Verify(
            repository => repository.GetBySlugAsync(animalSlug, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, AdoptrixAutoData]
    public async Task GetAnimal_WithUnknownAnimalId_ShouldReturnNotFound(GetAnimalRequest request)
    {
        // arrange
        fixture.AnimalsRepositoryMock
            .Setup(repository => repository.GetBySlugAsync(request.AnimalIdOrSlug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Animal);

        // act
        var testResult = await fixture.Client.GETAsync<GetAnimalEndpoint, GetAnimalRequest, AnimalResponse>(request);

        // assert
        testResult.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
