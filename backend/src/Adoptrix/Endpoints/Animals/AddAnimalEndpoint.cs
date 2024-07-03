using Adoptrix.Domain.Contracts.Requests.Animals;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Animals;

public class AddAnimalEndpoint(IAnimalsService animalsService) : Endpoint<AddAnimalRequest, Results<Created<AnimalResponse>, BadRequest>>
{
    public override void Configure()
    {
        Post("animals");
    }

    public override async Task<Results<Created<AnimalResponse>, BadRequest>> ExecuteAsync(AddAnimalRequest request, CancellationToken cancellationToken)
    {
        var result = await animalsService.AddAsync(request, cancellationToken);

        return TypedResults.Created("", result.Value); // TODO: Add location header
    }
}
