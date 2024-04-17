using Adoptrix.Application.Features.Animals.Commands;
using Adoptrix.Domain.Models;
using FluentValidation;

namespace Adoptrix.Application.Features.Animals.Validators;

public class AnimalImageFileDataValidator : AbstractValidator<AnimalImageFileData>
{
    private static readonly string[] ValidFileExtensions = [".jpg", ".jpeg", ".png"];
    private static readonly string[] ValidContentTypes = ["image/jpeg", "image/png"];

    public AnimalImageFileDataValidator()
    {
        RuleFor(fileData => fileData.ContentType)
            .MaximumLength(AnimalImage.ContentTypeMaxLength)
            .Must(contentType => ValidContentTypes.Contains(contentType))
            .WithMessage($"Invalid content type: {{PropertyValue}}. Valid content types are: {string.Join(", ", ValidContentTypes)}");

        RuleFor(fileData => fileData.FileName)
            .Must(fileName => ValidFileExtensions.Contains(Path.GetExtension(fileName)))
            .WithMessage(
                $"Invalid file extension: {{PropertyValue}}. Valid file extensions are: {string.Join(", ", ValidFileExtensions)}");

        RuleFor(fileData => fileData.Length)
            .GreaterThanOrEqualTo(1024)
            .WithMessage("File size must be greater than 1KB");
    }
}
