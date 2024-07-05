using Adoptrix.Application.Features.Breeds.Validators;
using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Domain;
using Adoptrix.Domain.Contracts.Requests.Breeds;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Application.Tests.Features.Breeds.Validators;

public class AddBreedCommandValidatorTests
{
    [Theory, AdoptrixAutoData]
    public async Task ValidateAsync_WithValidCommand_ShouldBeValid(
        [Frozen] Mock<ISpeciesRepository> speciesRepositoryMock,
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        Species existingSpecies,
        AddBreedCommandValidator validator, AddBreedRequest request)
    {
        // arrange
        speciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingSpecies);
        breedsRepositoryMock
            .Setup(repository => repository.GetByNameAsync(request.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var result = await validator.ValidateAsync(request);

        // assert
        result.IsValid.Should().BeTrue();
    }

    [Theory, AdoptrixAutoData]
    public async Task ValidateAsync_WithExistingBreedName_ShouldBeInvalid(
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        AddBreedCommandValidator validator, AddBreedRequest request, Breed existingBreed)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByNameAsync(request.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBreed);

        // act
        var result = await validator.ValidateAsync(request);

        // assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(failure => failure.PropertyName == nameof(request.Name))
            .Which.ErrorMessage.Should().Be($"Breed with name: '{request.Name}' already exists");
    }

    [Theory, AdoptrixAutoData]
    public async Task ValidateAsync_WithUnknownSpeciesId_ShouldBeInvalid(
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        [Frozen] Mock<ISpeciesRepository> speciesRepositoryMock,
        AddBreedCommandValidator validator, AddBreedRequest request)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByNameAsync(request.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);
        speciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Species);

        // act
        var result = await validator.ValidateAsync(request);

        // assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(failure => failure.PropertyName == nameof(request.SpeciesId))
            .Which.ErrorMessage.Should().Be($"Could not find species with ID: '{request.SpeciesId}'");
    }
}
