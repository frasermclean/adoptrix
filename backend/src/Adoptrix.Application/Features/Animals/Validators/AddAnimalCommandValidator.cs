using Adoptrix.Application.Features.Animals.Commands;
using Adoptrix.Application.Validators;
using Adoptrix.Domain.Models;
using FluentValidation;

namespace Adoptrix.Application.Features.Animals.Validators;

public class AddAnimalCommandValidator : AbstractValidator<AddAnimalCommand>
{
    public AddAnimalCommandValidator(DateOfBirthValidator dateOfBirthValidator, BreedIdValidator breedIdValidator)
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(Animal.NameMaxLength);

        RuleFor(request => request.Description)
            .MaximumLength(Animal.DescriptionMaxLength);

        RuleFor(request => request.BreedId)
            .SetValidator(breedIdValidator);

        RuleFor(request => request.DateOfBirth)
            .SetValidator(dateOfBirthValidator);
    }
}
