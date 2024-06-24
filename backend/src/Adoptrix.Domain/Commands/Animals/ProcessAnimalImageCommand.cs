using FluentResults;
using MediatR;

namespace Adoptrix.Domain.Commands.Animals;

public record ProcessAnimalImageCommand(Guid AnimalId, Guid ImageId) : IRequest<Result>;
