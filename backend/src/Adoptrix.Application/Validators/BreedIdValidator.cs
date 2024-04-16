using Adoptrix.Application.Services;
using FluentValidation;

namespace Adoptrix.Application.Validators;

public class BreedIdValidator : AbstractValidator<Guid>
{
    public BreedIdValidator(IBreedsRepository breedsRepository)
    {
        RuleFor(breedId => breedId)
            .MustAsync(async (breedId, cancellationToken) =>
            {
                var breed = await breedsRepository.GetByIdAsync(breedId, cancellationToken);
                return breed is not null;
            }).WithMessage("Could not find breed with ID: '{PropertyValue}'");
    }
}
