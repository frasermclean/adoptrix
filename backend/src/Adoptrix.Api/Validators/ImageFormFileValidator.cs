using FluentValidation;

namespace Adoptrix.Api.Validators;

public class ImageFormFileValidator : AbstractValidator<IFormFile>
{
    private static readonly string[] ValidFileExtensions = [".jpg", ".jpeg", ".png"];
    private static readonly string[] ValidContentTypes = ["image/jpeg", "image/png"];

    public ImageFormFileValidator()
    {
        RuleFor(formFile => formFile.ContentType)
            .Must(contentType => ValidContentTypes.Contains(contentType))
            .WithMessage($"Invalid content type: {{PropertyValue}}. Valid content types are: {string.Join(", ", ValidContentTypes)}");

        RuleFor(formFile => formFile.FileName)
            .Must(fileName => ValidFileExtensions.Contains(Path.GetExtension(fileName)))
            .WithMessage(
                $"Invalid file extension: {{PropertyName}}. Valid file extensions are: {string.Join(", ", ValidFileExtensions)}");
    }
}
