using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands.Animals;

public class DeleteAnimalCommand : ICommand<Result>
{
    public required Guid Id { get; init; }
}