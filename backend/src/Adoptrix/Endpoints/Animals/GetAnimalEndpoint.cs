using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Animals;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Animals;

public class GetAnimalEndpoint(ISender sender) : Endpoint<GetAnimalQuery, Results<Ok<AnimalResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("animals/{animalId}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(GetAnimalQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}
