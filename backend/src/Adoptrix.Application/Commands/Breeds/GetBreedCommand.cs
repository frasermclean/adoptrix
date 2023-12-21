using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands.Breeds;

public sealed class GetBreedCommand : ICommand<Result<Breed>>
{
    public required Guid Id { get; init; }
}