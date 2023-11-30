using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands.Breeds;

public class DeleteBreedCommand : ICommand<Result>
{
    public required int Id { get; init; }
}