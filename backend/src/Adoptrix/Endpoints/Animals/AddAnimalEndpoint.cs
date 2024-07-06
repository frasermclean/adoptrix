using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Core.Services;
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
