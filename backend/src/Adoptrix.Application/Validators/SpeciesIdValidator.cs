using Adoptrix.Application.Services;
using FluentValidation;

namespace Adoptrix.Application.Validators;

public class SpeciesIdValidator : AbstractValidator<Guid>
{
    public SpeciesIdValidator(ISpeciesRepository speciesRepository)
    {
        RuleFor(speciesId => speciesId)
            .MustAsync(async (speciesId, cancellationToken) =>
            {
                var species = await speciesRepository.GetByIdAsync(speciesId, cancellationToken);
                return species is not null;
            }).WithMessage("Could not find species with ID: '{PropertyValue}'");
    }
}
