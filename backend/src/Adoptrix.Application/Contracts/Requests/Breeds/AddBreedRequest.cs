using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Contracts.Requests.Breeds;

public record AddBreedRequest(string Name, Guid SpeciesId, Guid UserId) : IRequest<Result<Breed>>;
