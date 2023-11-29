using Adoptrix.Api.Validators;
using Adoptrix.Application.Commands;
using Adoptrix.Domain;
using FastEndpoints;
using FluentValidation;

namespace Adoptrix.Api.Endpoints.Animals.AddAnimal;

public class AddAnimalCommandValidator : Validator<AddAnimalCommand>
{
    public AddAnimalCommandValidator(DateOfBirthValidator dateOfBirthValidator)
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(Animal.NameMaxLength);

        RuleFor(command => command.Description)
            .MaximumLength(Animal.DescriptionMaxLength);

        RuleFor(command => command.DateOfBirth)
            .SetValidator(dateOfBirthValidator);
    }
}