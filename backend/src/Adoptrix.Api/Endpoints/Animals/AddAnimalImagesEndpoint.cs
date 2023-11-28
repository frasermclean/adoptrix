using Adoptrix.Api.Contracts;
using Adoptrix.Api.Validators;
using Adoptrix.Application.Commands;
using Adoptrix.Application.Services;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FastEndpoints;
using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public class AddAnimalImagesEndpoint(ImageContentTypeValidator contentTypeValidator, ISqidConverter sqidConverter)
    : Endpoint<AddAnimalImagesRequest, Results<Ok<Animal>, NotFound, BadRequest<IEnumerable<string>>>>
{
    public override void Configure()
    {
        Post("animals/{id}/images");
        AllowFileUploads(true);
    }

    public override async Task<Results<Ok<Animal>, NotFound, BadRequest<IEnumerable<string>>>> ExecuteAsync(
        AddAnimalImagesRequest request, CancellationToken cancellationToken)
    {
        var animalId = sqidConverter.CovertToInt(request.Id);
        var getResult = await new GetAnimalCommand { Id = animalId }.ExecuteAsync(cancellationToken);
        if (getResult.IsFailed)
        {
            return TypedResults.NotFound();
        }

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

            // execute command
            var commandResult = await new AddAnimalImageCommand
            {
                Animal = getResult.Value,
                FileStream = section.Section.Body,
                ContentType = section.Section.ContentType!,
                Description = section.Name,
                OriginalFileName = section.FileName
            }.ExecuteAsync(cancellationToken);

            results.Add(commandResult);
        }

        return results.TrueForAll(result => result.IsSuccess)
            ? TypedResults.Ok(getResult.Value)
            : TypedResults.BadRequest(results.Where(result => result.IsFailed)
                .Select(result => result.Errors.First().Message));
    }
}