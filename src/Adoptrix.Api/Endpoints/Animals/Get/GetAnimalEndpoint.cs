using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.Get;

[HttpGet("animals/{id}")]
public class GetAnimalEndpoint(IAnimalsRepository repository)
    : Endpoint<GetAnimalRequest, Results<Ok<Animal>, NotFound>>
{
    public override async Task<Results<Ok<Animal>, NotFound>> ExecuteAsync(GetAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetAsync(request.Id, cancellationToken);
        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}