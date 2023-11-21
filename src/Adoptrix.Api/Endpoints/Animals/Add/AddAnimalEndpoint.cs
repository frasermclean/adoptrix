using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.Add;

[HttpPost("animals")]
public class AddAnimalEndpoint(IAnimalsRepository repository)
    : Endpoint<AddAnimalRequest, Created<Animal>>
{
    public override async Task<Created<Animal>> ExecuteAsync(AddAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var result = await repository.AddAsync(new Animal
        {
            Name = request.Name,
            Species = request.Species,
            DateOfBirth = request.DateOfBirth
        }, cancellationToken);

        var animal = result.Value;
        return TypedResults.Created($"api/animals/{animal.Id}", animal);
    }
}