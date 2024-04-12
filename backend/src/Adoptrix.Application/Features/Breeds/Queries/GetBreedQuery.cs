using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Breeds.Queries;

public record GetBreedQuery(string BreedIdOrName) : IRequest<Result<Breed>>;
