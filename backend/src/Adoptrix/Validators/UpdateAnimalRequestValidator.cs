using Adoptrix.Domain.Contracts.Requests.Animals;
using Adoptrix.Domain.Models;
using FastEndpoints;
using FluentValidation;

namespace Adoptrix.Validators;

public class UpdateAnimalRequestValidator : Validator<UpdateAnimalRequest>
{
    public UpdateAnimalRequestValidator()
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
