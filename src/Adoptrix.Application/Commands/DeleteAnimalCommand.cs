using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands;

public class DeleteAnimalCommand : ICommand<Result>
{
    public required Guid Id { get; init; }
}