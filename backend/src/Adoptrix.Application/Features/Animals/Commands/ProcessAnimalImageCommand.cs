using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Commands;

public record ProcessAnimalImageCommand(Guid AnimalId, Guid ImageId) : IRequest<Result>;
