using Adoptrix.Api.Validators;
using Adoptrix.Application.Commands.Animals;
using Adoptrix.Domain;
using FastEndpoints;
using FluentValidation;

namespace Adoptrix.Api.Endpoints.Animals.UpdateAnimal;

public class UpdateAnimalCommandValidator : Validator<UpdateAnimalCommand>
{
    public UpdateAnimalCommandValidator(DateOfBirthValidator dateOfBirthValidator)
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