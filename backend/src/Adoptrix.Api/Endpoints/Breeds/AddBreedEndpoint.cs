﻿using System.Security.Claims;
using Adoptrix.Api.Contracts.Data;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Contracts.Requests.Breeds;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public class AddBreedEndpoint
{
    public static async Task<Results<Created<BreedResponse>, ValidationProblem>> ExecuteAsync(
        SetBreedData data,
        ClaimsPrincipal claimsPrincipal,
        ISender sender,
        ILogger<AddBreedEndpoint> logger,
        IValidator<SetBreedData> validator,
        LinkGenerator linkGenerator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(data, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation failed for request: {Request}", data);
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await sender.Send(
            new AddBreedRequest(data.Name, data.SpeciesId, claimsPrincipal.GetUserId()),
            cancellationToken);

        var response = result.Value.ToResponse();
        return TypedResults.Created(linkGenerator.GetPathByName(GetBreedEndpoint.EndpointName, new
        {
            breedIdOrName = response.Id
        }), response);
    }
}
