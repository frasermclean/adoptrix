using Adoptrix.Api.Validators;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;

namespace Adoptrix.Api.Tests.Validators;

public class ImageFormFileValidatorTests
{
    private readonly ImageFormFileValidator validator = new();
    private readonly Mock<IFormFile> formFileMock = new();

    [Fact]
    public void ValidImageFormFile_Should_Not_HaveAnyErrors()
    {
        // arrange
        formFileMock.SetupGet(file => file.ContentType).Returns("image/jpeg");
        formFileMock.SetupGet(file => file.FileName).Returns("image.jpg");

        // act
        var result = validator.TestValidate(formFileMock.Object);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void InvalidImageFormFile_Should_HaveErrors()
    {
        // arrange
        formFileMock.SetupGet(file => file.ContentType).Returns("application/pdf");
        formFileMock.SetupGet(file => file.FileName).Returns("document.pdf");

        // act
        var result = validator.TestValidate(formFileMock.Object);

        // assert
        result.ShouldHaveValidationErrorFor(file => file.ContentType);
        result.ShouldHaveValidationErrorFor(file => file.FileName);
    }
}
