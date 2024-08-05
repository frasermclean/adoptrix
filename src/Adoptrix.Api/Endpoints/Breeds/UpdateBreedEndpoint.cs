﻿using Adoptrix.Api.Mapping;
using Adoptrix.Api.Security;
using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public class UpdateBreedEndpoint(IBreedsRepository breedsRepository, ISpeciesRepository speciesRepository)
    : Endpoint<UpdateBreedRequest, Results<Ok<BreedResponse>, NotFound, ErrorResponse>>
{
    public override void Configure()
    {
        Put("breeds/{breedId:int}");
        Permissions(PermissionNames.BreedsWrite);
    }

    public override async Task<Results<Ok<BreedResponse>, NotFound, ErrorResponse>> ExecuteAsync(UpdateBreedRequest request, CancellationToken cancellationToken)
    {
        var breed = await breedsRepository.GetByIdAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            return TypedResults.NotFound();
        }

        var species = await speciesRepository.GetAsync(request.SpeciesName, cancellationToken);
        if (species is null)
        {
            AddError(r => r.SpeciesName, "Invalid species name");
            return new ErrorResponse(ValidationFailures);
        }

        breed.Name = request.Name;
        breed.Species = species;
        await breedsRepository.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(breed.ToResponse());
    }
}
