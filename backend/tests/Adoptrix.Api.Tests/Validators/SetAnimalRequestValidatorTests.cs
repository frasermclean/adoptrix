﻿using Adoptrix.Api.Validators;
using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using Adoptrix.Domain.Models.Factories;
using FluentValidation.TestHelper;

namespace Adoptrix.Api.Tests.Validators;

public class SetAnimalRequestValidatorTests
{
    private readonly SetAnimalRequestValidator validator;

    public SetAnimalRequestValidatorTests()
    {
        var speciesRepositoryMock = new Mock<ISpeciesRepository>();
        speciesRepositoryMock.Setup(repository =>
                repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid speciesId, CancellationToken _) => speciesId == Guid.Empty
                ? null
                : SpeciesFactory.Create(speciesId));


        var breedsRepositoryMock = new Mock<IBreedsRepository>();
        breedsRepositoryMock.Setup(service =>
                service.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid breedId, CancellationToken _) => breedId == Guid.Empty
                ? null
                : BreedFactory.Create(breedId));

        validator = new SetAnimalRequestValidator(new DateOfBirthValidator(), speciesRepositoryMock.Object,
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
        var request = CreateRequest(speciesId: Guid.Empty);


        // act
        var result = await validator.TestValidateAsync(request);

        // assert
        result.ShouldHaveValidationErrorFor(r => r.SpeciesId)
            .WithErrorMessage($"Could not find species with ID: {Guid.Empty}");
    }

    [Fact]
    public async Task WhenBreedId_IsInvalid_Should_HaveError()
    {
        // arrange
        var request = CreateRequest(breedId: Guid.Empty);

        // act
        var result = await validator.TestValidateAsync(request);

        // assert
        result.ShouldHaveValidationErrorFor(r => r.BreedId)
            .WithErrorMessage($"Could not find breed with ID: {Guid.Empty}");
    }

    private static SetAnimalRequest CreateRequest(string name = "Max", string description = "A good boy", int age = 2,
        Guid? speciesId = null, Guid? breedId = null, Sex sex = Sex.Male) => new()
    {
        Name = name,
        Description = description,
        DateOfBirth = DateOnly.FromDateTime(DateTime.Now - TimeSpan.FromDays(365 * age)),
        SpeciesId = speciesId ?? Guid.NewGuid(),
        BreedId = breedId ?? Guid.NewGuid(),
        Sex = sex
    };
}
