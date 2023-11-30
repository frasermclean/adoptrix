using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;
using Microsoft.Identity.Web;

namespace Adoptrix.Application.Commands.Breeds;

public class AddBreedCommand : ICommand<Result<Breed>>
{
    public required string Name { get; init; }
    public required string Species { get; init; }

    [FromClaim(ClaimConstants.NameIdentifierId)]
    public Guid UserId { get; init; }
}