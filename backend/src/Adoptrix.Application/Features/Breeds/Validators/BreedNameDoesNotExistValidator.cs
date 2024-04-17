using Adoptrix.Application.Services;
using FluentValidation;

namespace Adoptrix.Application.Features.Breeds.Validators;

public class BreedNameDoesNotExistValidator : AbstractValidator<string>
{
    public BreedNameDoesNotExistValidator(IBreedsRepository breedsRepository)
    {
        RuleFor(breedName => breedName)
            .NotEmpty()
            .MustAsync(async (breedName, cancellationToken) =>
            {
                var breed = await breedsRepository.GetByNameAsync(breedName, cancellationToken);
                return breed is null;
            })
            .WithMessage("Breed with name: '{PropertyValue}' already exists");
    }
}
