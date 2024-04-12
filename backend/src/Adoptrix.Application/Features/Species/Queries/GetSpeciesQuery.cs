using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Species.Queries;

public record GetSpeciesQuery(string SpeciesIdOrName) : IRequest<Result<Domain.Models.Species>>;
