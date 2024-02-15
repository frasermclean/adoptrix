using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using InvalidOperationException = System.InvalidOperationException;

namespace Adoptrix.Api.Endpoints.Animals;

public sealed class AddAnimalEndpoint
{
    public static async Task<Results<Created<AnimalResponse>, BadRequest<ValidationFailedResponse>>> ExecuteAsync(
        AddAnimalRequest request,
        HttpContext context,
        IValidator<AddAnimalRequest> validator,
        ILogger<AddAnimalEndpoint> logger,
        IAnimalsService animalsService,
        CancellationToken cancellationToken)
    {
        // validate request
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation failed for request: {Request}", request);
            return TypedResults.BadRequest(new ValidationFailedResponse());
        }

        var addAnimalResult = await animalsService.AddAnimalAsync(request.Name, request.Description, request.SpeciesName, request.BreedName,
            request.Sex, request.DateOfBirth, context.User.GetUserId(), cancellationToken);

        if (addAnimalResult.IsFailed)
        {
            logger.LogError("Could not add animal with name {Name}", request.Name);
            throw new InvalidOperationException(addAnimalResult.GetFirstErrorMessage());
        }

        var response = addAnimalResult.Value.ToResponse();

        logger.LogInformation("Added animal with id {Id}", addAnimalResult.Value.Id);
        return TypedResults.Created($"api/animals/{response.Id}", response);
    }
}
