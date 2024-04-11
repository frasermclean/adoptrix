using FluentResults;
using MediatR;

namespace Adoptrix.Application.Contracts.Requests.Animals;

public record CleanupAnimalImagesRequest(Guid AnimalId) : IRequest<Result>;
