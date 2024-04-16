using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Application.Features.Animals.Validators;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentValidation;

namespace Adoptrix.Api.Validators;

public sealed class SetAnimalDataValidator : AbstractValidator<SetAnimalRequest>
{
    public SetAnimalDataValidator(DateOfBirthValidator dateOfBirthValidator, IBreedsRepository breedsRepository)
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
