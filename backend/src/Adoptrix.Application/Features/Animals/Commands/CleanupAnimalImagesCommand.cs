using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Commands;

public record CleanupAnimalImagesCommand(Guid AnimalId) : IRequest<Result>;
