using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Features.Breeds.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public class UpdateBreedEndpoint
{
    public static async Task<Results<Ok<BreedResponse>, NotFound, ValidationProblem>> ExecuteAsync(
        Guid breedId,
        SetBreedRequest request,
        IValidator<SetBreedRequest> validator,
        ILogger<UpdateBreedEndpoint> logger,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation failed for request: {Request}", request);
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await sender.Send(new UpdateBreedCommand(breedId, request.Name, request.SpeciesId),
            cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.ToResponse())
            : TypedResults.NotFound();
    }
}
