﻿using Adoptrix.Core;
using Adoptrix.Core.Abstractions;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Mapping;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Breeds;

[HttpPost("breeds")]
public class AddBreedEndpoint(IBreedsRepository breedsRepository, ISpeciesRepository speciesRepository)
    : Endpoint<AddBreedRequest, Results<Created<BreedResponse>, ErrorResponse>>
{
    public override async Task<Results<Created<BreedResponse>, ErrorResponse>> ExecuteAsync(AddBreedRequest request,
        CancellationToken cancellationToken)
    {
        var species = await speciesRepository.GetByIdAsync(request.SpeciesId, cancellationToken);
        if (species is null)
        {
            AddError(r => r.SpeciesId, "Invalid species ID");
            return new ErrorResponse(ValidationFailures);
        }

        var breed = MapToBreed(request, species);
        await breedsRepository.AddAsync(breed, cancellationToken);

        return TypedResults.Created($"/api/breeds/{breed.Id}", breed.ToResponse());
    }

    private static Breed MapToBreed(AddBreedRequest request, Core.Species species) => new()
    {
        Name = request.Name,
        Species = species,
        CreatedBy = request.UserId
    };
}