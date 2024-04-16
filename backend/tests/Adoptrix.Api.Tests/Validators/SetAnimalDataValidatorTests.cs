using Adoptrix.Api.Contracts.Data;
using Adoptrix.Api.Validators;
using Adoptrix.Application.Features.Animals.Validators;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared.Factories;
using FluentValidation.TestHelper;

namespace Adoptrix.Api.Tests.Validators;

public class SetAnimalDataValidatorTests
{
    private readonly SetAnimalDataValidator validator;

    public SetAnimalDataValidatorTests()
    {
        var breedsRepositoryMock = new Mock<IBreedsRepository>();
        breedsRepositoryMock.Setup(service =>
                service.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid breedId, CancellationToken _) => breedId == Guid.Empty
                ? null
                : BreedFactory.Create(breedId));

        validator = new SetAnimalDataValidator(new DateOfBirthValidator(), breedsRepositoryMock.Object);
    }

    [Fact]
    public async Task ValidRequest_Should_Not_HaveAnyErrors()
    {
        // arrange
        var data = CreateData();

        // act
        var result = await validator.TestValidateAsync(data);

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
        var data = CreateData(name);

        // act
        var result = await validator.TestValidateAsync(data);

        // assert
        result.ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage(expectedErrorMessage);
    }

    [Fact]
    public async Task WhenDateOfBirth_IsInTheFuture_Should_HaveError()
    {
        // arrange
        var data = CreateData(age: -1);

        // act
        var result = await validator.TestValidateAsync(data);

        // assert
        result.ShouldHaveValidationErrorFor(r => r.DateOfBirth);
    }

    [Fact]
    public async Task WhenBreedId_IsInvalid_Should_HaveError()
    {
        // arrange
        var request = CreateData(breedId: Guid.Empty);

        // act
        var result = await validator.TestValidateAsync(request);

        // assert
        result.ShouldHaveValidationErrorFor(r => r.BreedId)
            .WithErrorMessage($"Could not find breed with ID: {Guid.Empty}");
    }

    private static SetAnimalData CreateData(
        string name = "Max",
        string description = "A good boy",
        Guid? breedId = null,
        Sex sex = Sex.Male,
        int age = 2)
        => new(
            name,
            description,
            breedId ?? Guid.NewGuid(),
            sex,
            DateOnly.FromDateTime(DateTime.Now - TimeSpan.FromDays(365 * age)));
}
