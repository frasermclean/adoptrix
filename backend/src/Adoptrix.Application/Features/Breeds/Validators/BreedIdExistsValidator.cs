using Adoptrix.Application.Services;
using FluentValidation;

namespace Adoptrix.Application.Features.Breeds.Validators;

public class BreedIdExistsValidator : AbstractValidator<Guid>
{
    public BreedIdExistsValidator(IBreedsRepository breedsRepository)
    {
        RuleFor(breedId => breedId)
            .NotEmpty()
            .MustAsync(async (breedId, cancellationToken) =>
            {
                var breed = await breedsRepository.GetByIdAsync(breedId, cancellationToken);
                return breed is not null;
            }).WithMessage("Could not find breed with ID: '{PropertyValue}'");
    }
}
