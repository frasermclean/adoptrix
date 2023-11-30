using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands.Animals;

public class GetAnimalCommand : ICommand<Result<Animal>>
{
    public required string Id { get; init; }
}