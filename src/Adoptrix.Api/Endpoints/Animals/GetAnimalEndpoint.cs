using Adoptrix.Api.Mapping;
using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

[HttpGet("animals/{animalIdOrSlug}"), AllowAnonymous]
public class GetAnimalEndpoint(IAnimalsRepository animalsRepository)
    : Endpoint<GetAnimalRequest, Results<Ok<AnimalResponse>, NotFound>>
{
    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(GetAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var animal = int.TryParse(request.AnimalIdOrSlug, out var animalId)
            ? await animalsRepository.GetByIdAsync(animalId, cancellationToken)
            : await animalsRepository.GetBySlugAsync(request.AnimalIdOrSlug, cancellationToken);

        return animal is not null
            ? TypedResults.Ok(animal.ToResponse())
            : TypedResults.NotFound();
    }
}
