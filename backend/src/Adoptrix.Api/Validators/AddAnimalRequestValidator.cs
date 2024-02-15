using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FluentValidation;

namespace Adoptrix.Api.Validators;

public sealed class AddAnimalRequestValidator : AbstractValidator<AddAnimalRequest>
{
    public AddAnimalRequestValidator(DateOfBirthValidator dateOfBirthValidator, ISpeciesRepository speciesRepository,
        IBreedsRepository breedsRepository)
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(Animal.NameMaxLength);

        RuleFor(request => request.Description)
            .MaximumLength(Animal.DescriptionMaxLength);

        RuleFor(request => request.SpeciesName)
            .NotEmpty()
            .MaximumLength(Species.NameMaxLength)
            .MustAsync(async (speciesName, cancellationToken) =>
            {
                var result = await speciesRepository.GetByNameAsync(speciesName, cancellationToken);
                return result.IsSuccess;
            })
            .WithMessage("Could not find species with name: {PropertyValue}");

        RuleFor(request => request.BreedName)
            .MaximumLength(Breed.NameMaxLength)
            .MustAsync(async (breedName, cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(breedName))
                {
                    return true;
                }

                var result = await breedsRepository.GetByNameAsync(breedName, cancellationToken);
                return result.IsSuccess;
            })
            .WithMessage("Could not find breed with name: {PropertyValue}");

        RuleFor(request => request.DateOfBirth)
            .SetValidator(dateOfBirthValidator);
    }
}
