using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Application.Contracts.Requests;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentValidation;

namespace Adoptrix.Api.Validators;

public class SetBreedRequestValidator : AbstractValidator<SetBreedRequest>
{
    public SetBreedRequestValidator(IBreedsService breedsService, ISpeciesRepository speciesRepository)
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
                var result = await speciesRepository.GetByIdAsync(speciesId, cancellationToken);
                return result.IsSuccess;
            })
            .WithMessage("Could not find species with ID: {PropertyValue}");
    }
}
