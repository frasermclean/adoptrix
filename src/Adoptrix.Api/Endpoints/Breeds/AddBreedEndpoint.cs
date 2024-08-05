using Adoptrix.Api.Mapping;
using Adoptrix.Api.Security;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public class AddBreedEndpoint(IBreedsRepository breedsRepository, ISpeciesRepository speciesRepository)
    : Endpoint<AddBreedRequest, Results<Created<BreedResponse>, ErrorResponse>>
{
    public override void Configure()
    {
        Post("breeds");
        Permissions(PermissionNames.BreedsWrite);
    }

    public override async Task<Results<Created<BreedResponse>, ErrorResponse>> ExecuteAsync(AddBreedRequest request,
        CancellationToken cancellationToken)
    {
        var species = await speciesRepository.GetAsync(request.SpeciesName, cancellationToken);
        if (species is null)
        {
            AddError(r => r.SpeciesName, "Invalid species name");
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
