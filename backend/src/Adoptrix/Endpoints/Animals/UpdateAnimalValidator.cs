using Adoptrix.Core;
using Adoptrix.Validators;
using FastEndpoints;
using FluentValidation;

namespace Adoptrix.Endpoints.Animals;

public class UpdateAnimalValidator : Validator<UpdateAnimalRequest>
{
    public UpdateAnimalValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(Animal.NameMaxLength);

        RuleFor(request => request.Description)
            .MaximumLength(Animal.DescriptionMaxLength);

        RuleFor(request => request.BreedId)
            .NotEmpty();

        RuleFor(request => request.Sex)
            .IsInEnum();

        RuleFor(request => request.DateOfBirth)
            .SetValidator(new DateOfBirthValidator());
    }
}
