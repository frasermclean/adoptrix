using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands;

public class UpdateAnimalCommand : ICommand<Result<Animal>>
{
    public string Id { get; init; } = string.Empty;
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required Species Species { get; init; }
    public required DateOnly DateOfBirth { get; init; }
}