using Adoptrix.Application.Features.Breeds.Commands;
using Adoptrix.Application.Services;
using Adoptrix.Application.Validators;
using FluentValidation;

namespace Adoptrix.Application.Features.Breeds.Validators;

public class AddBreedCommandValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedCommandValidator(IBreedsRepository breedsRepository, SpeciesIdValidator speciesIdValidator)
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MustAsync(async (name, cancellationToken) =>
            {
                var breed = await breedsRepository.GetByNameAsync(name, cancellationToken);
                return breed is null;
            })
            .WithMessage("Breed with name: '{PropertyValue}' already exists");

        RuleFor(request => request.SpeciesId)
            .NotEmpty()
            .SetValidator(speciesIdValidator);
    }
}
