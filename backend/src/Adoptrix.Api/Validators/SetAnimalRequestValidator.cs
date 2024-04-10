using Adoptrix.Api.Contracts.Data;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using FluentValidation;

namespace Adoptrix.Api.Validators;

public sealed class SetAnimalRequestValidator : AbstractValidator<SetAnimalData>
{
    public SetAnimalRequestValidator(DateOfBirthValidator dateOfBirthValidator, ISpeciesRepository speciesRepository,
        IBreedsRepository breedsRepository)
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(Animal.NameMaxLength);

        RuleFor(request => request.Description)
            .MaximumLength(Animal.DescriptionMaxLength);

        RuleFor(request => request.BreedId)
            .MustAsync(async (breedId, cancellationToken) =>
            {
                var breed = await breedsRepository.GetByIdAsync(breedId, cancellationToken);
                return breed is not null;
            })
            .WithMessage("Could not find breed with ID: {PropertyValue}");

        RuleFor(request => request.DateOfBirth)
            .SetValidator(dateOfBirthValidator);
    }
}
