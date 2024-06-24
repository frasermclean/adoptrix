using FluentResults;
using MediatR;

namespace Adoptrix.Domain.Commands.Animals;

public record CleanupAnimalImagesCommand(Guid AnimalId) : IRequest<Result>;
