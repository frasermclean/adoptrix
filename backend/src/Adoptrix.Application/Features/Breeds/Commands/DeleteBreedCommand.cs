using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Breeds.Commands;

public record DeleteBreedCommand(Guid BreedId) : IRequest<Result>;
