using FluentValidation;

namespace Adoptrix.Api.Validators;

public class ImagesFormFileCollectionValidator : AbstractValidator<IFormFileCollection>
{
    public ImagesFormFileCollectionValidator(ImageFormFileValidator imageFormFileValidator)
    {
        RuleForEach(collection => collection)
            .NotEmpty()
            .WithName("Images")
            .WithMessage("At least one image is required")
            .SetValidator(imageFormFileValidator);
    }
}
