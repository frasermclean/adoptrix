using Adoptrix.Api.Security;
using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Errors;
using Adoptrix.Logic.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public class AddBreedEndpoint(IBreedsService breedsService)
    : Endpoint<AddBreedRequest, Results<Created<BreedResponse>, ErrorResponse>>
{
    public override void Configure()
    {
        Post("breeds");
        Permissions(PermissionNames.BreedsWrite);
    }

    public override async Task<Results<Created<BreedResponse>, ErrorResponse>> ExecuteAsync(AddBreedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await breedsService.AddAsync(request, cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Created($"/api/breeds/{result.Value.Id}", result.Value);
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
