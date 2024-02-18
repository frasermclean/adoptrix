using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Validators;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FluentValidation.TestHelper;

namespace Adoptrix.Api.Tests.Validators;

public class AddAnimalRequestValidatorTests
{
    private readonly AddAnimalRequestValidator validator;

    private const string UnknownSpeciesName = "unknown-species";
    private const string UnknownBreedName = "unknown-breed";

    public AddAnimalRequestValidatorTests()
    {
        var speciesRepositoryMock = new Mock<ISpeciesRepository>();
        speciesRepositoryMock.Setup(service =>
                service.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string speciesName, CancellationToken _) => speciesName == UnknownSpeciesName
                ? new SpeciesNotFoundError(speciesName)
                : new Species
                {
                    Name = speciesName
                });

        var breedsRepositoryMock = new Mock<IBreedsRepository>();
        breedsRepositoryMock.Setup(service =>
                service.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string breedName, CancellationToken _) => breedName == UnknownBreedName
                ? new BreedNotFoundError(breedName)
                : new Breed
                {
                    Name = breedName,
                    Species = new Species()
                });


        validator = new AddAnimalRequestValidator(new DateOfBirthValidator(), speciesRepositoryMock.Object,
            breedsRepositoryMock.Object);
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

    [Theory]
    [InlineData("", "'Name' must not be empty.")]
    [InlineData("123456789012345678901234567890123456789012345678901",
        "The length of 'Name' must be 50 characters or fewer. You entered 51 characters.")]
    public async Task WhenName_IsInvalid_Should_HaveValidationError(string name, string expectedErrorMessage)
    {
        // arrange
        var request = CreateRequest(name);

        // act
        var result = await validator.TestValidateAsync(request);

        // assert
        result.ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage(expectedErrorMessage);
    }

    [Fact]
    public async Task WhenDateOfBirth_IsInTheFuture_Should_HaveError()
    {
        // arrange
        var request = CreateRequest(age: -1);

        // act
        var result = await validator.TestValidateAsync(request);

        // assert
        result.ShouldHaveValidationErrorFor(r => r.DateOfBirth);
    }

    [Fact]
    public async Task WhenSpeciesName_IsInvalid_Should_HaveError()
    {
        // arrange
        var request = CreateRequest(speciesName: UnknownSpeciesName);


        // act
        var result = await validator.TestValidateAsync(request);

        // assert
        result.ShouldHaveValidationErrorFor(r => r.SpeciesName)
            .WithErrorMessage($"Could not find species with name: {UnknownSpeciesName}");
    }

    [Fact]
    public async Task WhenBreedName_IsInvalid_Should_HaveError()
    {
        // arrange
        var request = CreateRequest(breedName: UnknownBreedName);

        // act
        var result = await validator.TestValidateAsync(request);

        // assert
        result.ShouldHaveValidationErrorFor(r => r.BreedName)
            .WithErrorMessage($"Could not find breed with name: {UnknownBreedName}");
    }

    private static SetAnimalRequest CreateRequest(string name = "Max", string description = "A good boy", int age = 2,
        string speciesName = "dog", string breedName = "Labrador", Sex sex = Sex.Male) => new()
    {
        Name = name,
        Description = description,
        DateOfBirth = DateOnly.FromDateTime(DateTime.Now - TimeSpan.FromDays(365 * age)),
        SpeciesName = speciesName,
        BreedName = breedName,
        Sex = sex
    };
}
