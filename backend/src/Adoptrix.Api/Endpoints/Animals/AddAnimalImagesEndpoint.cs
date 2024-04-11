using System.Security.Claims;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Api.Validators;
using Adoptrix.Application.Contracts.Requests.Animals;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public static class AddAnimalImagesEndpoint
{
    public static async Task<Results<Ok<AnimalResponse>, NotFound, ValidationProblem>> ExecuteAsync(
        Guid animalId,
        IFormFileCollection formFileCollection,
        ImagesFormFileCollectionValidator validator,
        ClaimsPrincipal claimsPrincipal,
        ISender sender,
        CancellationToken cancellationToken)
    {
        // ensure animal exists
        var getResult = await sender.Send(new GetAnimalRequest(animalId), cancellationToken);
        if (getResult.IsFailed)
        {
            return TypedResults.NotFound();
        }

        // validate form files
        var validationResult = await validator.ValidateAsync(formFileCollection, cancellationToken);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await sender.Send(new AddAnimalImagesRequest(
                animalId,
                claimsPrincipal.GetUserId(),
                formFileCollection.Select(formFile => new AnimalImageFileData(
                    formFile.FileName,
                    formFile.Name,
                    formFile.ContentType,
                    formFile.Length,
                    formFile.OpenReadStream()))),
            cancellationToken);

        return TypedResults.Ok(result.Value.ToResponse());
    }
}
