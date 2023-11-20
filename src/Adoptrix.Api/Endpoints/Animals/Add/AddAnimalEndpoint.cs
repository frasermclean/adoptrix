using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.Add;

public class AddAnimalEndpoint(IAnimalsRepository animalsRepository)
    : Endpoint<AddAnimalRequest, Results<Created<Animal>, BadRequest>>
{
    public override void Configure()
    {
        Post("animals");
        AllowAnonymous();
    }

    public override async Task<Results<Created<Animal>, BadRequest>> ExecuteAsync(AddAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var animal = await animalsRepository.AddAsync(new Animal
        {
            Name = request.Name,
            Species = request.Species,
            DateOfBirth = request.DateOfBirth
        }, cancellationToken);

        var uri = new Uri($"/api/animals/{animal.Id}", UriKind.Relative);
        return TypedResults.Created(uri, animal);
    }
}