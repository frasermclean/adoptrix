using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Domain.Queries.Breeds;

public record GetBreedQuery(Guid BreedId) : IRequest<Result<Breed>>;
