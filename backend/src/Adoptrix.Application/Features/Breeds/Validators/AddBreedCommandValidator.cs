using Adoptrix.Application.Features.Breeds.Commands;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentValidation;

namespace Adoptrix.Application.Features.Breeds.Validators;

public class AddBreedCommandValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedCommandValidator(IBreedsRepository breedsRepository, ISpeciesRepository speciesRepository)
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(Breed.NameMaxLength)
            .MustAsync(async (name, cancellationToken) =>
            {
                var breed = await breedsRepository.GetByNameAsync(name, cancellationToken);
                return breed is null;
            })
            .WithMessage("Breed with name: '{PropertyValue}' already exists");

        RuleFor(request => request.SpeciesId)
            .NotEmpty()
            .MustAsync(async (speciesId, cancellationToken) =>
            {
                var species = await speciesRepository.GetByIdAsync(speciesId, cancellationToken);
                return species is not null;
            })
            .WithMessage("Could not find species with ID: {PropertyValue}");
    }
}
