using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands;

public class GetAnimalCommand : ICommand<Result<Animal>>
{
    public required int Id { get; init; }
}