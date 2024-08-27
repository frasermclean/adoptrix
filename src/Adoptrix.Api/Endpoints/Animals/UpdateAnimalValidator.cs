using Adoptrix.Api.Validators;
using Adoptrix.Contracts.Requests;
using Adoptrix.Core;
using FluentValidation;

namespace Adoptrix.Api.Endpoints.Animals;

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
            .Must(value => Enum.TryParse<Sex>(value, out _))
            .WithMessage("Invalid value");

        RuleFor(request => request.DateOfBirth)
            .SetValidator(new DateOfBirthValidator());
    }
}
