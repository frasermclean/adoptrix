using Adoptrix.Api.Security;
using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Errors;
using Adoptrix.Logic.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public class UpdateBreedEndpoint(IBreedsService breedsService)
    : Endpoint<UpdateBreedRequest, Results<Ok<BreedResponse>, NotFound, ErrorResponse>>
{
    public override void Configure()
    {
        Put("breeds/{breedId:int}");
        Permissions(PermissionNames.BreedsWrite);
    }

    public override async Task<Results<Ok<BreedResponse>, NotFound, ErrorResponse>> ExecuteAsync(
        UpdateBreedRequest request, CancellationToken cancellationToken)
    {
        var result = await breedsService.UpdateAsync(request, cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        if (result.HasError<BreedNotFoundError>())
        {
            return TypedResults.NotFound();
        }

        if (result.HasError<DuplicateBreedError>())
        {
            AddError(r => r.Name, $"Breed with name '{request.Name}' already exists");
            return new ErrorResponse(ValidationFailures, StatusCodes.Status409Conflict);
        }

        if (result.HasError<SpeciesNotFoundError>())
        {
            AddError(r => r.SpeciesName, "Invalid species name");
        }

        return new ErrorResponse(ValidationFailures);
    }
}
