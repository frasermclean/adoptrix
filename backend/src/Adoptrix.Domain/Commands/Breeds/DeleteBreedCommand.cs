using FluentResults;
using MediatR;

namespace Adoptrix.Domain.Commands.Breeds;

public record DeleteBreedCommand(Guid BreedId) : IRequest<Result>;
