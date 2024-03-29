﻿using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
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
                var result = await breedsRepository.GetByNameAsync(name, cancellationToken);
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
