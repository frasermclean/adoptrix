﻿using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Breeds;

public class AddBreedRequest
{
    public required string Name { get; init; }
    public Guid SpeciesId { get; init; }

    [FromClaim(ClaimTypes.UserId)] public Guid UserId { get; init; }
}