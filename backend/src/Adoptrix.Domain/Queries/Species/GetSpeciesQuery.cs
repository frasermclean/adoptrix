using FluentResults;
using MediatR;
using SpeciesModel = Adoptrix.Domain.Models.Species;

namespace Adoptrix.Domain.Queries.Species;

public record GetSpeciesQuery(Guid SpeciesId) : IRequest<Result<SpeciesModel>>;
