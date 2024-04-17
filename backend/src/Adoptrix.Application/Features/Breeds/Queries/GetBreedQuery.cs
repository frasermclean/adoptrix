using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Breeds.Queries;

public record GetBreedQuery(Guid BreedId) : IRequest<Result<Breed>>;
