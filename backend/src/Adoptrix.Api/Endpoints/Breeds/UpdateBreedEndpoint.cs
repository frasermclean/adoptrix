using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Contracts.Requests;
using Adoptrix.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public class UpdateBreedEndpoint
{
    public static async
        Task<Results<Ok<BreedResponse>, NotFound, ValidationProblem>> ExecuteAsync(
            Guid breedId,
            SetBreedRequest request,
            IValidator<SetBreedRequest> validator,
            ILogger<UpdateBreedEndpoint> logger,
            IBreedsService breedsService,
            ISpeciesRepository speciesRepository,
            CancellationToken cancellationToken)
    {
        // find the breed by id
        var getResult = await breedsService.GetByIdAsync(breedId, cancellationToken);
        if (getResult.IsFailed)
        {
            return TypedResults.NotFound();
        }

        // validate request
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation failed for request: {Request}", request);
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var speciesResult = await speciesRepository.GetByIdAsync(request.SpeciesId, cancellationToken);

        // update breed properties
        var breed = getResult.Value;
        breed.Name = request.Name;
        breed.Species = speciesResult.Value;

        await breedsService.UpdateAsync(breed, cancellationToken);

        return TypedResults.Ok(breed.ToResponse());
    }
}
