using Adoptrix.Domain;
using FastEndpoints;
using FluentValidation;

namespace Adoptrix.Api.Endpoints.Animals.Add;

public class AddAnimalValidator : Validator<AddAnimalRequest>
{
    public AddAnimalValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(Animal.NameMaxLength);
    }
}