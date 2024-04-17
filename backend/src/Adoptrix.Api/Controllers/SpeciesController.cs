using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Features.Species.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adoptrix.Api.Controllers;

public class SpeciesController(ISender sender) : ApiController
{
    [HttpGet, AllowAnonymous]
    public async Task<IEnumerable<SpeciesResponse>> GetAllSpecies(CancellationToken cancellationToken)
    {
        var allSpecies = await sender.Send(new GetAllSpeciesQuery(), cancellationToken);
        return allSpecies.Select(species => species.ToResponse());
    }

    [HttpGet("{speciesId:guid}"), AllowAnonymous]
    public async Task<IActionResult> GetSpecies(Guid speciesId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetSpeciesQuery(speciesId), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value.ToResponse())
            : Problem(result.Errors);
    }
}
