using FluentResults;
using MediatR;

namespace Adoptrix.Application.Contracts.Requests.Animals;

public record DeleteAnimalRequest(Guid AnimalId) : IRequest<Result>;
