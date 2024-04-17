using Adoptrix.Application.Features.Animals.Commands;
using FluentValidation;

namespace Adoptrix.Application.Features.Animals.Validators;

public class AddAnimalImagesCommandValidator : AbstractValidator<AddAnimalImagesCommand>
{
    public AddAnimalImagesCommandValidator(AnimalImageFileDataValidator animalImageFileDataValidator)
    {
        RuleFor(command => command.AnimalId).NotEmpty();
        RuleFor(command => command.UserId).NotEmpty();

        RuleFor(command => command.FileData)
            .NotEmpty()
            .WithMessage("At least one image is required");

        RuleForEach(command => command.FileData)
            .SetValidator(animalImageFileDataValidator);
    }
}
