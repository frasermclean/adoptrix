using Adoptrix.Api.Security;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Logic.Errors;
using Adoptrix.Logic.Services;
using Microsoft.AspNetCore.Http.HttpResults;

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
