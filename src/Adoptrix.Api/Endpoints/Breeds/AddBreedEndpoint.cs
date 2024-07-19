using Adoptrix.Core;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using BreedMapper = Adoptrix.Api.Mapping.BreedMapper;

namespace Adoptrix.Api.Endpoints.Breeds;

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

        return TypedResults.Created($"/api/breeds/{breed.Id}", BreedMapper.ToResponse(breed));
    }

    private static Breed MapToBreed(AddBreedRequest request, Core.Species species) => new()
    {
        Name = request.Name,
        Species = species,
        CreatedBy = request.UserId
    };
}
