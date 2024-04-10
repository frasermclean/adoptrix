using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Contracts.Requests.Animals;

public record UpdateAnimalRequest(
    Guid AnimalId,
    string Name,
    string? Description,
    Guid BreedId,
    Sex Sex,
    DateOnly DateOfBirth
    ) : IRequest<Result<Animal>>;
