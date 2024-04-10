using FluentResults;
using MediatR;

namespace Adoptrix.Application.Contracts.Requests.Species;

public record GetSpeciesRequest(string SpeciesIdOrName) : IRequest<Result<Domain.Models.Species>>;
