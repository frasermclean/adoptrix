using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Domain.Commands.Animals;

public record AddAnimalCommand(
    string Name,
    string? Description,
    Guid BreedId,
    Sex Sex,
    DateOnly DateOfBirth,
    Guid UserId) : IRequest<Result<Animal>>;
