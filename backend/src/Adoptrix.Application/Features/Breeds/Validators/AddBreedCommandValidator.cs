using Adoptrix.Application.Features.Species.Validators;
using Adoptrix.Core.Contracts.Requests.Breeds;
using FluentValidation;

namespace Adoptrix.Application.Features.Breeds.Validators;

public class AddBreedCommandValidator : AbstractValidator<AddBreedRequest>
{
    public AddBreedCommandValidator(BreedNameDoesNotExistValidator breedNameDoesNotExistValidator,
        SpeciesIdExistsValidator speciesIdExistsValidator)
    {
        RuleFor(command => command.Name)
            .SetValidator(breedNameDoesNotExistValidator);

        RuleFor(command => command.SpeciesId)
            .SetValidator(speciesIdExistsValidator);
    }
}
