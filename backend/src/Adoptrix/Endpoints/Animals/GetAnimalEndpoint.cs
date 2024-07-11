using Adoptrix.Mapping;
using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Core.Contracts.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Animals;

public class GetAnimalEndpoint(IAnimalsRepository animalsRepository)
    : Endpoint<GetAnimalRequest, Results<Ok<AnimalResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("animals/{animalId:guid}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(GetAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var animal = await animalsRepository.GetByIdAsync(request.AnimalId, cancellationToken);
        if (animal is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(animal.ToResponse());
    }
}
