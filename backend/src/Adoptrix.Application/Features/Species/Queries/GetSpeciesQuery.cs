using FluentResults;
using MediatR;
using SpeciesModel = Adoptrix.Domain.Models.Species;

namespace Adoptrix.Application.Features.Species.Queries;

public record GetSpeciesQuery(Guid SpeciesId) : IRequest<Result<SpeciesModel>>;
