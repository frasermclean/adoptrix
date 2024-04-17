using Adoptrix.Application.Features.Breeds.Commands;
using Adoptrix.Application.Features.Species.Validators;
using FluentValidation;

namespace Adoptrix.Application.Features.Breeds.Validators;

public class AddBreedCommandValidator : AbstractValidator<AddBreedCommand>
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
