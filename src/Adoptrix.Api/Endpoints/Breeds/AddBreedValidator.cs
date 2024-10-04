using Adoptrix.Core;
using Adoptrix.Core.Requests;
using FluentValidation;

namespace Adoptrix.Api.Endpoints.Breeds;

public class AddBreedValidator : Validator<AddBreedRequest>
{
    public AddBreedValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(Breed.NameMaxLength);

        RuleFor(request => request.SpeciesName)
            .NotEmpty();
    }
}
