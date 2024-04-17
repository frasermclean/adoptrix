using Adoptrix.Application.Features.Breeds.Commands;
using Adoptrix.Application.Features.Species.Validators;
using FluentValidation;

namespace Adoptrix.Application.Features.Breeds.Validators;

public class UpdateBreedCommandValidator : AbstractValidator<UpdateBreedCommand>
{
    public UpdateBreedCommandValidator(BreedNameDoesNotExistValidator breedNameDoesNotExistValidator,
        SpeciesIdExistsValidator speciesIdExistsValidator)
    {
        RuleFor(command => command.BreedName)
            .SetValidator(breedNameDoesNotExistValidator);

        RuleFor(request => request.SpeciesId)
            .SetValidator(speciesIdExistsValidator);
    }
}
