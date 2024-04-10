using Adoptrix.Application.Contracts.Requests.Breeds;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using FluentValidation;

namespace Adoptrix.Api.Validators;

public class SetBreedRequestValidator : AbstractValidator<SetBreedRequest>
{
    public SetBreedRequestValidator(IBreedsRepository breedsRepository, ISpeciesRepository speciesRepository)
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
