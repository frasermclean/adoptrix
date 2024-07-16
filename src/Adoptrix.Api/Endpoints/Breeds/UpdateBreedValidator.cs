using Adoptrix.Core;
using FastEndpoints;
using FluentValidation;

namespace Adoptrix.Api.Endpoints.Breeds;

public class UpdateBreedValidator : Validator<UpdateBreedRequest>
{
    public UpdateBreedValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(Breed.NameMaxLength);

        RuleFor(request => request.SpeciesId)
            .NotEmpty();
    }
}
