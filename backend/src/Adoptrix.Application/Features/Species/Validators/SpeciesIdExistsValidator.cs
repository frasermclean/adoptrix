using Adoptrix.Application.Services.Abstractions;
using FluentValidation;

namespace Adoptrix.Application.Features.Species.Validators;

public class SpeciesIdExistsValidator : AbstractValidator<Guid>
{
    public SpeciesIdExistsValidator(ISpeciesRepository speciesRepository)
    {
        RuleFor(speciesId => speciesId)
            .NotEmpty()
            .MustAsync(async (speciesId, cancellationToken) =>
            {
                var species = await speciesRepository.GetByIdAsync(speciesId, cancellationToken);
                return species is not null;
            }).WithMessage("Could not find species with ID: '{PropertyValue}'");
    }
}
