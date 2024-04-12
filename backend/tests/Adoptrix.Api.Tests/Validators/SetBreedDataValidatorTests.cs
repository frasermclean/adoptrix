using Adoptrix.Api.Contracts.Data;
using Adoptrix.Api.Validators;
using Adoptrix.Application.Services;
using Adoptrix.Tests.Shared.Factories;
using FluentValidation.TestHelper;

namespace Adoptrix.Api.Tests.Validators;

public class SetBreedDataValidatorTests
{
    private readonly SetBreedDataValidator validator;

    private const string ExistingBreedName = "Sausage Dog";

    public SetBreedDataValidatorTests()
    {
        var breedsRepositoryMock = new Mock<IBreedsRepository>();
        var speciesRepositoryMock = new Mock<ISpeciesRepository>();

        breedsRepositoryMock.Setup(repository =>
                repository.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string breedName, CancellationToken _) => breedName != ExistingBreedName
                ? null
                : BreedFactory.Create(name: breedName));

        speciesRepositoryMock.Setup(repository =>
                repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid speciesId, CancellationToken _) => speciesId == Guid.Empty
                ? null
                : SpeciesFactory.Create(speciesId));

        validator = new SetBreedDataValidator(breedsRepositoryMock.Object, speciesRepositoryMock.Object);
    }

    [Fact]
    public async Task ValidRequest_Should_Not_HaveAnyErrors()
    {
        // arrange
        var request = CreateData();

        // act
        var result = await validator.TestValidateAsync(request);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Request_WithExistingName_Should_HaveError()
    {
        // arrange
        var request = CreateData(ExistingBreedName);

        // act
        var result = await validator.TestValidateAsync(request);

        // assert
        result.ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage($"Breed with name: '{ExistingBreedName}' already exists");
    }

    [Fact]
    public async Task Request_WithNonExistingSpeciesId_Should_HaveError()
    {
        // arrange
        var request = CreateData(speciesId: Guid.Empty);

        // act
        var result = await validator.TestValidateAsync(request);

        // assert
        result.ShouldHaveValidationErrorFor(r => r.SpeciesId)
            .WithErrorMessage($"Could not find species with ID: {Guid.Empty}");
    }

    private static SetBreedData CreateData(string name = "Corgi", Guid? speciesId = null)
        => new(name, speciesId ?? Guid.NewGuid());
}
