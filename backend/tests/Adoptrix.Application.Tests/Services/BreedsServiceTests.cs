using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Domain.Contracts.Requests.Breeds;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Errors;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared;
using Adoptrix.Tests.Shared.Factories;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Tests.Services;

public class BreedsServiceTests
{
    private readonly Mock<ILogger<BreedsService>> loggerMock = new();
    private readonly Mock<IBreedsRepository> breedsRepositoryMock = new();
    private readonly Mock<ISpeciesRepository> speciesRepositoryMock = new();
    private readonly BreedsService breedsService;

    public BreedsServiceTests()
    {
        breedsService = new BreedsService(loggerMock.Object, breedsRepositoryMock.Object, speciesRepositoryMock.Object);
    }

    [Theory, AdoptrixAutoData]
    public async Task AddAsync_WithValidRequest_ShouldReturnSuccess(AddBreedRequest request)
    {
        // arrange
        speciesRepositoryMock.Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(SpeciesFactory.Create(request.SpeciesId));

        // act
        var result = await breedsService.AddAsync(request);

        // assert
        result.Should().BeSuccess().Which.Value.Should().BeOfType<BreedResponse>();
    }

    [Theory, AdoptrixAutoData]
    public async Task AddAsync_WithInvalidSpeciesId_ShouldReturnError(AddBreedRequest request)
    {
        // arrange
        speciesRepositoryMock.Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Species);

        // act
        var result = await breedsService.AddAsync(request);

        // assert
        result.Should().BeFailure().Which.HasError<SpeciesNotFoundError>();
    }

    [Theory, AdoptrixAutoData]
    public async Task UpdateAsync_WithValidRequest_ShouldReturnSuccess(UpdateBreedRequest request)
    {
        // arrange
        breedsRepositoryMock.Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BreedFactory.Create(request.BreedId));
        speciesRepositoryMock.Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(SpeciesFactory.Create(request.SpeciesId));

        // act
        var result = await breedsService.UpdateAsync(request);

        // assert
        result.Should().BeSuccess().Which.Value.Should().BeOfType<BreedResponse>();
    }

    [Theory, AdoptrixAutoData]
    public async Task UpdateAsync_WithInvalidBreedId_ShouldReturnError(UpdateBreedRequest request)
    {
        // arrange
        breedsRepositoryMock.Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var result = await breedsService.UpdateAsync(request);

        // assert
        result.Should().BeFailure().Which.HasError<BreedNotFoundError>();
    }

    [Theory, AdoptrixAutoData]
    public async Task UpdateAsync_WithInvalidSpeciesId_ShouldReturnError(UpdateBreedRequest request)
    {
        // arrange
        breedsRepositoryMock.Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BreedFactory.Create(request.BreedId));
        speciesRepositoryMock.Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Species);

        // act
        var result = await breedsService.UpdateAsync(request);

        // assert
        result.Should().BeFailure().Which.HasError<SpeciesNotFoundError>();
    }
}
