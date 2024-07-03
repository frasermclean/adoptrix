using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Extensions;
using Adoptrix.Domain.Commands.Breeds;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BreedMapper = Adoptrix.Application.Mapping.BreedMapper;

namespace Adoptrix.Api.Controllers;

public class BreedsController(ISender sender) : ApiController
{

    [HttpPost]
    public async Task<IActionResult> AddBreed(SetBreedRequest request, CancellationToken cancellationToken)
    {
        var command = new AddBreedCommand(request.Name, request.SpeciesId, User.GetUserId());
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetBreed), new { breedId = result.Value.Id }, BreedMapper.ToResponse(result.Value))
            : Problem(result.Errors);
    }

    [HttpPut("{breedId:guid}")]
    public async Task<IActionResult> UpdateBreed(Guid breedId, SetBreedRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateBreedCommand(breedId, request.Name, request.SpeciesId);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(BreedMapper.ToResponse(result.Value))
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
