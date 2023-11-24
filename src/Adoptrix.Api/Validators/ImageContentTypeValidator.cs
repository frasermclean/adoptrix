using FluentValidation;

namespace Adoptrix.Api.Validators;

public class ImageContentTypeValidator : AbstractValidator<string?>
{
    private static readonly string[] ValidContentTypes = { "image/jpeg", "image/png" };

    public ImageContentTypeValidator()
    {
        RuleFor(contentType => contentType)
            .Must(contentType => ValidContentTypes.Contains(contentType))
            .WithMessage($"Invalid content type. Valid content types are: {string.Join(", ", ValidContentTypes)}");
    }
}