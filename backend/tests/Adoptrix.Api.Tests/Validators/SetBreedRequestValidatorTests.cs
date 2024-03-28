using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Validators;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using Adoptrix.Domain;
using FluentResults;
using FluentValidation.TestHelper;

namespace Adoptrix.Api.Tests.Validators;

public class SetBreedRequestValidatorTests
{
    private readonly SetBreedRequestValidator validator;

    private const string ExistingBreedName = "Sausage Dog";

    public SetBreedRequestValidatorTests()
    {
        var breedsRepositoryMock = new Mock<IBreedsRepository>();
        var speciesRepositoryMock = new Mock<ISpeciesRepository>();

        breedsRepositoryMock.Setup(repository =>
                repository.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string breedName, CancellationToken _) => breedName != ExistingBreedName
                ? new BreedNotFoundError(breedName)
                : Result.Ok(new Breed
                {
                    Name = breedName, Species = new Species()
                }));

        speciesRepositoryMock.Setup(repository =>
                repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid speciesId, CancellationToken _) => speciesId == Guid.Empty
                ? new SpeciesNotFoundError(speciesId)
                : Result.Ok(new Species
                {
                    Id = speciesId
                }));

        validator = new SetBreedRequestValidator(breedsRepositoryMock.Object, speciesRepositoryMock.Object);
    }

    [Fact]
    public async Task ValidRequest_Should_Not_HaveAnyErrors()
    {
        // arrange
        var request = CreateRequest();

        // act
        var result = await validator.TestValidateAsync(request);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Request_WithExistingName_Should_HaveError()
    {
        // arrange
        var request = CreateRequest(ExistingBreedName);

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
        var request = CreateRequest(speciesId: Guid.Empty);

        // act
        var result = await validator.TestValidateAsync(request);

        // assert
        result.ShouldHaveValidationErrorFor(r => r.SpeciesId)
            .WithErrorMessage($"Could not find species with ID: {Guid.Empty}");
    }

    private static SetBreedRequest CreateRequest(string name = "Corgi", Guid? speciesId = null) => new()
    {
        Name = name, SpeciesId = speciesId ?? Guid.NewGuid()
    };
}
