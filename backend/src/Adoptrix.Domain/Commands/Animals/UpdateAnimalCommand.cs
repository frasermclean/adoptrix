using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Domain.Commands.Animals;

public record UpdateAnimalCommand(
    Guid AnimalId,
    string Name,
    string? Description,
    Guid BreedId,
    Sex Sex,
    DateOnly DateOfBirth) : IRequest<Result<Animal>>;
