using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Contracts.Requests.Breeds;

public record GetBreedRequest(string BreedIdOrName) : IRequest<Result<Breed>>;
