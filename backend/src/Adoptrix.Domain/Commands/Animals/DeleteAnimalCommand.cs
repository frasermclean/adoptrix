using FluentResults;
using MediatR;

namespace Adoptrix.Domain.Commands.Animals;

public record DeleteAnimalCommand(Guid AnimalId) : IRequest<Result>;
