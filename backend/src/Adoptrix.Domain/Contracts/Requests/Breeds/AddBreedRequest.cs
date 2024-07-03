using Adoptrix.Domain.Models;
using FastEndpoints;
using FluentResults;
using MediatR;

namespace Adoptrix.Domain.Contracts.Requests.Breeds;

public class AddBreedRequest : IRequest<Result<Breed>>
{
    public required string Name { get; init; }
    public Guid SpeciesId { get; init; }

    [FromClaim(ClaimTypes.UserId)] public Guid UserId { get; init; }
}
