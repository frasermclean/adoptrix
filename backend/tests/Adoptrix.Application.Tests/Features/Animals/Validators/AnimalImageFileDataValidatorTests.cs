using Adoptrix.Application.Features.Animals.Commands;
using Adoptrix.Application.Features.Animals.Validators;
using FluentValidation.TestHelper;

namespace Adoptrix.Application.Tests.Features.Animals.Validators;

public class AnimalImageFileDataValidatorTests
{
    private readonly AnimalImageFileDataValidator validator = new();

    [Fact]
    public void Validate_WithValidData_ShouldNotHaveAnyErrors()
    {
        // arrange
        var data = CreateData();

        // act
        var result = validator.TestValidate(data);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithInvalidContentType_ShouldHaveError()
    {
        // arrange
        var data = CreateData(contentType: "application/pdf");

        // act
        var result = validator.TestValidate(data);

        // assert
        result.ShouldHaveValidationErrorFor(file => file.ContentType);
    }

    [Fact]
    public void Validate_WithInvalidFileName_ShouldHaveError()
    {
        // arrange
        var data = CreateData(fileName: "document.pdf");

        // act
        var result = validator.TestValidate(data);

        // assert
        result.ShouldHaveValidationErrorFor(file => file.FileName);
    }

    [Fact]
    public void Validate_WithInvalidLength_ShouldHaveError()
    {
        // arrange
        var data = CreateData(length: 512);

        // act
        var result = validator.TestValidate(data);

        // assert
        result.ShouldHaveValidationErrorFor(file => file.Length);
    }

    private static AnimalImageFileData CreateData(string fileName = "image.jpg", string description = "A cute dog",
        string contentType = "image/jpeg", long length = 2048)
    {
        return new AnimalImageFileData(fileName, description, contentType, length, new MemoryStream());
    }
}
