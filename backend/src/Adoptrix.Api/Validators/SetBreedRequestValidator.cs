using Adoptrix.Application.Contracts.Requests.Breeds;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentValidation;

namespace Adoptrix.Api.Validators;

public class SetBreedRequestValidator : AbstractValidator<SetBreedRequest>
{
    public SetBreedRequestValidator(IBreedsService breedsService, ISpeciesService speciesService)
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(Breed.NameMaxLength)
            .MustAsync(async (name, cancellationToken) =>
            {
                var result = await breedsService.GetByNameAsync(name, cancellationToken);
                return result.IsFailed;
            })
            .WithMessage("Breed with name: '{PropertyValue}' already exists");

        RuleFor(request => request.SpeciesId)
            .NotEmpty()
            .MustAsync(async (speciesId, cancellationToken) =>
            {
                var result = await speciesService.GetByIdAsync(speciesId, cancellationToken);
                return result.IsSuccess;
            })
            .WithMessage("Could not find species with ID: {PropertyValue}");
    }
}
