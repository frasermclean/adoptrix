using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Contracts.Requests.Breeds;

public record UpdateBreedRequest(Guid BreedId, string Name, Guid SpeciesId) : IRequest<Result<Breed>>;
