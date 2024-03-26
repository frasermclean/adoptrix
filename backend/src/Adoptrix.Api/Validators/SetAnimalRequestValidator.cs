using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FluentValidation;

namespace Adoptrix.Api.Validators;

public sealed class SetAnimalRequestValidator : AbstractValidator<SetAnimalRequest>
{
    public SetAnimalRequestValidator(DateOfBirthValidator dateOfBirthValidator, ISpeciesRepository speciesRepository,
        IBreedsRepository breedsRepository)
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(Animal.NameMaxLength);

        RuleFor(request => request.Description)
            .MaximumLength(Animal.DescriptionMaxLength);

        RuleFor(request => request.SpeciesId)
            .NotEmpty()
            .MustAsync(async (speciesId, cancellationToken) =>
            {
                var result = await speciesRepository.GetByIdAsync(speciesId, cancellationToken);
                return result.IsSuccess;
            })
            .WithMessage("Could not find species with ID: {PropertyValue}");

        RuleFor(request => request.BreedId)
            .MustAsync(async (breedId, cancellationToken) =>
            {
                var result = await breedsRepository.GetByIdAsync(breedId, cancellationToken);
                return result.IsSuccess;
            })
            .WithMessage("Could not find breed with ID: {PropertyValue}");

        RuleFor(request => request.DateOfBirth)
            .SetValidator(dateOfBirthValidator);
    }
}
