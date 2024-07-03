using Adoptrix.Application.Services;
using Adoptrix.Domain.Commands.Animals;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Extensions;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Animals.Images;

public class AddAnimalImagesEndpoint(IAnimalsService animalsService) : EndpointWithoutRequest<Results<Ok<AnimalResponse>, NotFound>>
{
    public override void Configure()
    {
        Post("animals/{animalId:guid}/images");
        AllowFileUploads();
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var command = new AddAnimalImagesCommand
        {
            AnimalId = Route<Guid>("animalId"),
            UserId = User.GetUserId(),
            FileData = Files.Select(formFile => new AnimalImageFileData
            {
                FileName = formFile.FileName,
                Description = formFile.Name,
                ContentType = formFile.ContentType,
                Length = formFile.Length,
                Stream = formFile.OpenReadStream()
            })
        };

        var result = await animalsService.AddImagesAsync(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}


