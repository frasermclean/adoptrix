using Adoptrix.Application.Features.Species.Responses;
using MediatR;

namespace Adoptrix.Application.Features.Species.Queries;

public record SearchSpeciesQuery : IRequest<IEnumerable<SearchSpeciesMatch>>;
