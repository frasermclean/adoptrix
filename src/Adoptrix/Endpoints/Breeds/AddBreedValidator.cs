using Adoptrix.Core;
using FastEndpoints;
using FluentValidation;

namespace Adoptrix.Endpoints.Breeds;

public class AddBreedValidator : Validator<AddBreedRequest>
{
    public AddBreedValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(Breed.NameMaxLength);

        RuleFor(request => request.SpeciesId)
            .NotEmpty();
    }
}
