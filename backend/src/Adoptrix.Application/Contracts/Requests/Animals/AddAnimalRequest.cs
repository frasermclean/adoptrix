using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Contracts.Requests.Animals;

public record AddAnimalRequest(
    string Name,
    string? Description,
    Guid BreedId,
    Sex Sex,
    DateOnly DateOfBirth,
    Guid UserId) : IRequest<Result<Animal>>;
