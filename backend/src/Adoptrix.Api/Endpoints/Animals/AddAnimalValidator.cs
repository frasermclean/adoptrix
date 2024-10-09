using Adoptrix.Api.Validators;
using Adoptrix.Core;
using FluentValidation;

namespace Adoptrix.Api.Endpoints.Animals;

public class AddAnimalValidator : Validator<AddAnimalRequest>
{
    public AddAnimalValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(Animal.NameMaxLength);

        RuleFor(request => request.Description)
            .MaximumLength(Animal.DescriptionMaxLength);

        RuleFor(request => request.BreedName)
            .NotEmpty();

        RuleFor(request => request.Sex)
            .IsInEnum();

        RuleFor(request => request.DateOfBirth)
            .SetValidator(new DateOfBirthValidator());

        RuleFor(request => request.UserId)
            .NotEmpty();
    }
}
