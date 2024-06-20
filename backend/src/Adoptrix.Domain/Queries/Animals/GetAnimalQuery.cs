using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Domain.Queries.Animals;

public record GetAnimalQuery(Guid AnimalId) : IRequest<Result<Animal>>;
