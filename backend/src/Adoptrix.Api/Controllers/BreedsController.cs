using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Features.Breeds.Commands;
using Adoptrix.Application.Features.Breeds.Queries;
using Adoptrix.Application.Features.Breeds.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adoptrix.Api.Controllers;

public class BreedsController(ISender sender) : ApiController
{
    [HttpGet, AllowAnonymous]
    public async Task<IEnumerable<SearchBreedsResult>> SearchBreeds([FromQuery] SearchBreedsQuery query,
        CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }

    [HttpGet("{breedId:guid}"), AllowAnonymous]
    public async Task<IActionResult> GetBreed(Guid breedId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetBreedQuery(breedId), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value.ToResponse())
            : Problem(result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> AddBreed(SetBreedRequest request, CancellationToken cancellationToken)
    {
        var command = new AddBreedCommand(request.Name, request.SpeciesId, User.GetUserId());
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetBreed), new { breedId = result.Value.Id }, result.Value.ToResponse())
            : Problem(result.Errors);
    }

    [HttpPut("{breedId:guid}")]
    public async Task<IActionResult> UpdateBreed(Guid breedId, SetBreedRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateBreedCommand(breedId, request.Name, request.SpeciesId);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value.ToResponse())
            : Problem(result.Errors);
    }

    [HttpDelete("{breedId:guid}")]
    public async Task<IActionResult> DeleteBreed(Guid breedId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteBreedCommand(breedId), cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : Problem(result.Errors);
    }
}
