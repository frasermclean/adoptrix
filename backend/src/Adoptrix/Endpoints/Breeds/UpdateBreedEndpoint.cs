using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Mapping;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Breeds;

public class UpdateBreedEndpoint(IBreedsRepository breedsRepository, ISpeciesRepository speciesRepository)
    : Endpoint<UpdateBreedRequest, Results<Ok<BreedResponse>, NotFound, ErrorResponse>>
{
    public override void Configure()
    {
        Put("breeds/{breedId:guid}");
    }

    public override async Task<Results<Ok<BreedResponse>, NotFound, ErrorResponse>> ExecuteAsync(UpdateBreedRequest request, CancellationToken cancellationToken)
    {
        var breed = await breedsRepository.GetByIdAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            return TypedResults.NotFound();
        }

        var species = await speciesRepository.GetByIdAsync(request.SpeciesId, cancellationToken);
        if (species is null)
        {
            AddError(r => r.SpeciesId, "Invalid species ID");
            return new ErrorResponse(ValidationFailures);
        }

        breed.Name = request.Name;
        breed.Species = species;
        await breedsRepository.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(breed.ToResponse());
    }
}
