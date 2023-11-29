using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands;

public class AddAnimalCommand : ICommand<Result<Animal>>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required string SpeciesName { get; init; }
    public required DateOnly DateOfBirth { get; init; }
}