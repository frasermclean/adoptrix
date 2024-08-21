using Adoptrix.Api.Security;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Logic.Errors;
using Adoptrix.Logic.Mapping;
using Adoptrix.Logic.Services;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Animals;

public class UpdateAnimalEndpoint(IAnimalsService animalsService)
    : Endpoint<UpdateAnimalRequest, Results<Ok<AnimalResponse>, NotFound, ErrorResponse>>
{
    public override void Configure()
    {
        Put("animals/{animalId:int}");
        Permissions(PermissionNames.AnimalsWrite);
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound, ErrorResponse>> ExecuteAsync(
        UpdateAnimalRequest request, CancellationToken cancellationToken)
    {
        var result = await animalsService.UpdateAsync(request, cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        if (result.HasError<AnimalNotFoundError>())
        {
            return TypedResults.NotFound();
        }

        if (result.HasError<BreedNotFoundError>())
        {
            AddError(r => r.BreedId, "Breed not found");
        }

        return new ErrorResponse(ValidationFailures);
    }
}
