using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands.Animals;

public class AddAnimalCommand : ICommand<Result<Animal>>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required string Species { get; init; }
    public string? Breed { get; init; }
    public required DateOnly DateOfBirth { get; init; }
}