using Adoptrix.Application.Features.Species.Validators;
using Adoptrix.Domain.Contracts.Requests.Breeds;
using FluentValidation;

namespace Adoptrix.Application.Features.Breeds.Validators;

public class UpdateBreedCommandValidator : AbstractValidator<UpdateBreedRequest>
{
    public UpdateBreedCommandValidator(BreedNameDoesNotExistValidator breedNameDoesNotExistValidator,
        SpeciesIdExistsValidator speciesIdExistsValidator)
    {
        RuleFor(command => command.Name)
            .SetValidator(breedNameDoesNotExistValidator);

        RuleFor(request => request.SpeciesId)
            .SetValidator(speciesIdExistsValidator);
    }
}
