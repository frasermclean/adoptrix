using Adoptrix.Core;
using FluentValidation;

namespace Adoptrix.Api.Endpoints.Breeds;

public class UpdateBreedValidator : Validator<UpdateBreedRequest>
{
    public UpdateBreedValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(Breed.NameMaxLength);

        RuleFor(request => request.SpeciesName)
            .NotEmpty();

        RuleFor(request => request.UserId)
            .NotEmpty();
    }
}
