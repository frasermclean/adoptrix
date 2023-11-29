using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands;

public class DeleteAnimalCommand : ICommand<Result>
{
    public required string Id { get; init; }
}