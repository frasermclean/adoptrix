using Adoptrix.Api.Mapping;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Species;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adoptrix.Api.Controllers;

public class SpeciesController(ISender sender) : ApiController
{
    [HttpGet, AllowAnonymous]
    public async Task<IEnumerable<SpeciesMatch>> SearchSpecies([FromQuery] SearchSpeciesQuery query,  CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
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
