using FluentResults;
using MediatR;

namespace Adoptrix.Application.Contracts.Requests.Breeds;

public record DeleteBreedRequest(Guid BreedId) : IRequest<Result>;
