using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Features.Animals.Commands;
using Adoptrix.Application.Features.Animals.Queries;
using Adoptrix.Application.Features.Animals.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adoptrix.Api.Controllers;

public class AnimalsController(ISender sender) : ApiController
{
    [HttpGet, AllowAnonymous]
    public async Task<IEnumerable<SearchAnimalsMatch>> SearchAnimals([FromQuery] SearchAnimalsQuery query,
        CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }

    [HttpGet("{animalIdOrSlug}"), AllowAnonymous]
    public async Task<IActionResult> GetAnimal(string animalIdOrSlug, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAnimalQuery(animalIdOrSlug), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value.ToResponse())
            : Problem(result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> AddAnimal(SetAnimalRequest request, CancellationToken cancellationToken)
    {
        var command = new AddAnimalCommand(request.Name, request.Description, request.BreedId, request.Sex,
            request.DateOfBirth, User.GetUserId());
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetAnimal), new { animalIdOrSlug = result.Value.Slug }, result.Value.ToResponse())
            : Problem(result.Errors);
    }

    [HttpPut("{animalId:guid}")]
    public async Task<IActionResult> UpdateAnimal(Guid animalId, SetAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateAnimalCommand(animalId, request.Name, request.Description, request.BreedId, request.Sex,
            request.DateOfBirth);

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value.ToResponse())
            : Problem(result.Errors);
    }

    [HttpDelete("{animalId:guid}")]
    public async Task<IActionResult> DeleteAnimal(Guid animalId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteAnimalCommand(animalId), cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : Problem(result.Errors);
    }

    [HttpPost("{animalId:guid}/images")]
    public async Task<IActionResult> AddAnimalImages(Guid animalId, IFormFileCollection formFileCollection,
        CancellationToken cancellationToken)
    {
        // TODO: Temporary workaround until https://github.com/dotnet/aspnetcore/issues/55174 is resolved
        if (formFileCollection.Count == 0)
        {
            var form = await Request.ReadFormAsync(cancellationToken);
            formFileCollection = form.Files;
        }

        var command = new AddAnimalImagesCommand(animalId, User.GetUserId(),
            formFileCollection.Select(formFile => new AnimalImageFileData(
                formFile.FileName,
                formFile.Name,
                formFile.ContentType,
                formFile.Length,
                formFile.OpenReadStream())));
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value.ToResponse())
            : Problem(result.Errors);
    }
}
