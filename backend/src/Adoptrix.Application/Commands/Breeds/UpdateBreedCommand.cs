using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands.Breeds;

public class UpdateBreedCommand : ICommand<Result<Breed>>
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
}