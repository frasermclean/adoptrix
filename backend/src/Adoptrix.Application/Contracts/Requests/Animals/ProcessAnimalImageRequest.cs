using FluentResults;
using MediatR;

namespace Adoptrix.Application.Contracts.Requests.Animals;

public record ProcessAnimalImageRequest(Guid AnimalId, Guid ImageId) : IRequest<Result>;
