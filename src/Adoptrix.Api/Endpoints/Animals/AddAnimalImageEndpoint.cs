using Adoptrix.Api.Requests;
using Adoptrix.Application.Commands;
using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public class AddAnimalImagesEndpoint
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
        var getResult = await new GetAnimalCommand { Id = request.Id }.ExecuteAsync(cancellationToken);
        if (getResult.IsFailed)
        {
            return TypedResults.NotFound();
        }

        var results = new List<Result>();
        await foreach (var section in FormFileSectionsAsync(cancellationToken))
        {
            if (section is null) continue;

            var command = new AddAnimalImageCommand
            {
                Animal = getResult.Value,
                FileStream = section.Section.Body,
                ContentType = section.Section.ContentType,
                Description = section.Name,
                OriginalFileName = section.FileName
            };
            var result = await command.ExecuteAsync(cancellationToken);
            if (result.IsFailed)
            {
                AddError(result.Errors.First().Message);
            }

            results.Add(result);
        }

        return results.TrueForAll(result => result.IsSuccess)
            ? TypedResults.Ok(getResult.Value)
            : TypedResults.BadRequest(results.Where(result => result.IsFailed)
                .Select(result => result.Errors.First().Message));
    }
}