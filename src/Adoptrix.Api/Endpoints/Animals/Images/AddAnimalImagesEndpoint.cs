using Adoptrix.Contracts.Responses;
using Adoptrix.Logic.Models;
using Adoptrix.Logic.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.Images;

public class AddAnimalImagesEndpoint(IAnimalImagesManager animalImagesManager)
    : Endpoint<AddAnimalImagesRequest, Results<Ok<AnimalResponse>, NotFound, ErrorResponse>>
{
    public override void Configure()
    {
        Post("animals/{animalId:int}/images");
        AllowFileUploads(true);
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound, ErrorResponse>> ExecuteAsync(
        AddAnimalImagesRequest request, CancellationToken cancellationToken)
    {
        var items = FormFileSectionsAsync(cancellationToken)
            .Select(section => new AddOriginalImageData
            {
                FileName = section!.FileName,
                Description = section.Name,
                ContentType = section.Section.ContentType ?? string.Empty,
                Stream = section.FileStream!
            });

        var result =
            await animalImagesManager.AddOriginalsAsync(request.AnimalId, request.UserId, items, cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        return TypedResults.NotFound();
    }
}
