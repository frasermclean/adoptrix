using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Commands;

public record DeleteAnimalCommand(Guid AnimalId) : IRequest<Result>;
