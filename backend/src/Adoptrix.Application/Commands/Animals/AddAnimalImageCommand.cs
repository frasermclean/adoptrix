using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands.Animals;

public class AddAnimalImageCommand : ICommand<Result>
{
    public required Animal Animal { get; init; }
    public required Stream FileStream { get; init; }
    public required string? Description { get; init; }
    public required string ContentType { get; init; }
    public required string FileName { get; init; }
    public Guid? UserId { get; set; }
}