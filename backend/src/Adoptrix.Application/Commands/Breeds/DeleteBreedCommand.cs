using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands.Breeds;

public class DeleteBreedCommand : ICommand<Result>
{
    public required Guid Id { get; init; }
}