﻿using Adoptrix.Api.Security;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Logic.Errors;
using Adoptrix.Logic.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public class AddAnimalEndpoint(IAnimalsService animalsService)
    : Endpoint<AddAnimalRequest, Results<Created<AnimalResponse>, ErrorResponse>>
{
    public override void Configure()
    {
        Post("animals");
        Permissions(PermissionNames.AnimalsWrite);
    }

    public override async Task<Results<Created<AnimalResponse>, ErrorResponse>> ExecuteAsync(AddAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var result = await animalsService.AddAsync(request, cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Created($"/api/animals/{result.Value.Id}", result.Value);
        }

        if (result.HasError<BreedNotFoundError>())
        {
            AddError(r => r.BreedId, "Breed not found");
        }

        return new ErrorResponse(ValidationFailures);
    }

    private static Animal MapToAnimal(AddAnimalRequest request, Breed breed) => new()
    {
        Name = request.Name,
        Description = request.Description,
        Breed = breed,
        Sex = Enum.Parse<Sex>(request.Sex),
        DateOfBirth = request.DateOfBirth,
        Slug = $"{request.Name.ToLower()}-{request.DateOfBirth:O}",
        CreatedBy = request.UserId
    };
}
