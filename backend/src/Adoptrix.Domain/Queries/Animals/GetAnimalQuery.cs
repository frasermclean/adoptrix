using Adoptrix.Domain.Models.Responses;
using FluentResults;
using MediatR;

namespace Adoptrix.Domain.Queries.Animals;

public record GetAnimalQuery(Guid AnimalId) : IRequest<Result<AnimalResponse>>;
