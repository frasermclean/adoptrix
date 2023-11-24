using Adoptrix.Application.Commands;
using Adoptrix.Domain;
using FastEndpoints;
using FluentValidation;

namespace Adoptrix.Api.Validators;

public class AddAnimalCommandValidator : Validator<AddAnimalCommand>
{
    public AddAnimalCommandValidator(DateOfBirthValidator dateOfBirthValidator)
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(Animal.NameMaxLength);

        RuleFor(request => request.Description)
            .MaximumLength(Animal.DescriptionMaxLength);

        RuleFor(request => request.DateOfBirth)
            .SetValidator(dateOfBirthValidator);
    }
}