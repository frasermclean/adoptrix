using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.Update;

[HttpPut("/animals/{id}")]
public class UpdateAnimalEndpoint(IAnimalsRepository repository)
    : Endpoint<UpdateAnimalRequest, Results<Ok<Animal>, NotFound>>
{
    public override async Task<Results<Ok<Animal>, NotFound>> ExecuteAsync(UpdateAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var result = await repository.UpdateAsync(new Animal
        {
            Id = request.Id,
            Name = request.Name,
            Species = request.Species,
            DateOfBirth = request.DateOfBirth
        }, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}