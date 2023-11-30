using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Services;
using Adoptrix.Api.Validators;
using Adoptrix.Application.Commands.Animals;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FastEndpoints;
using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.WebUtilities;

namespace Adoptrix.Api.Endpoints.Animals.AddAnimalImages;

public class AddAnimalImagesEndpoint(ImageContentTypeValidator contentTypeValidator,
        IResponseMappingService mappingService)
    : Endpoint<AddAnimalImagesRequest, Results<Ok<AnimalResponse>, NotFound, BadRequest<IEnumerable<string>>>>
{
    public override void Configure()
    {
        Post("animals/{id}/images");
        AllowFileUploads(true);
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound, BadRequest<IEnumerable<string>>>> ExecuteAsync(
        AddAnimalImagesRequest request, CancellationToken cancellationToken)
    {
        var getResult = await new GetAnimalCommand { Id = request.Id }.ExecuteAsync(cancellationToken);
        if (getResult.IsFailed)
        {
            return TypedResults.NotFound();
        }

        var animal = getResult.Value;
        var results = new List<Result>();
        await foreach (var section in FormFileSectionsAsync(cancellationToken))
        {
            if (section is null) continue;

            // validate content type
            var validationResult =
                await contentTypeValidator.ValidateAsync(section.Section.ContentType, cancellationToken);
            if (!validationResult.IsValid)
            {
                results.Add(new InvalidContentTypeError(section.Section.ContentType));
                continue;
            }

            results.Add(await ExecuteCommandAsync(animal, section, cancellationToken));
        }

        return results.TrueForAll(result => result.IsSuccess)
            ? TypedResults.Ok(mappingService.MapToResponse(animal))
            : TypedResults.BadRequest(results.Where(result => result.IsFailed)
                .Select(result => result.Errors.First().Message));
    }

    private static async Task<Result> ExecuteCommandAsync(Animal animal, FileMultipartSection section,
        CancellationToken cancellationToken)
    {
        var command = new AddAnimalImageCommand
        {
            Animal = animal,
            FileStream = section.Section.Body,
            ContentType = section.Section.ContentType!,
            Description = section.Name,
            OriginalFileName = section.FileName
        };

        return await command.ExecuteAsync(cancellationToken);
    }
}