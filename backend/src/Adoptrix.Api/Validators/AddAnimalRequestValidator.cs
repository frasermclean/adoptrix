using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Application.Services;
using FluentValidation;

namespace Adoptrix.Api.Validators;

public class AddAnimalRequestValidator : AbstractValidator<AddAnimalImagesRequest>
{
    public AddAnimalRequestValidator(ImageFormFileValidator imageFormFileValidator,
        IAnimalsRepository animalsRepository)
    {
        RuleFor(request => request.FormFileCollection)
            .NotEmpty()
            .WithMessage("At least one image is required");

        RuleForEach(request => request.FormFileCollection)
            .SetValidator(imageFormFileValidator);

        RuleForEach(request => request.FormFileCollection)
            .MustAsync(async (request, formFile, cancellationToken) =>
            {
                var animal = (await animalsRepository.GetAsync(request.AnimalId, cancellationToken)).Value;
                return animal.Images.All(image => image.OriginalFileName != formFile.FileName);
            })
            .WithMessage((_, formFile) => $"Image with file name {formFile.FileName} already exists");
    }
}
