using FluentValidation;

namespace Adoptrix.Api.Validators;

public class ImagesFormFileCollectionValidator : AbstractValidator<IFormFileCollection>
{
    private static readonly string[] ValidFileExtensions = [".jpg", ".jpeg", ".png"];
    private static readonly string[] ValidContentTypes = ["image/jpeg", "image/png"];

    public ImagesFormFileCollectionValidator(ImageFormFileValidator imageFormFileValidator)
    {
        RuleForEach(collection => collection)
            .NotEmpty()
            .WithName("Images")
            .WithMessage("At least one image is required")
            .SetValidator(imageFormFileValidator);
    }
}
