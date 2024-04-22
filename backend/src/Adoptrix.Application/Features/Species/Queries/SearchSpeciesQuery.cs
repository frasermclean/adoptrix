using Adoptrix.Application.Features.Species.Responses;
using MediatR;

namespace Adoptrix.Application.Features.Species.Queries;

public record SearchSpeciesQuery(bool WithAnimals = false) : IRequest<IEnumerable<SearchSpeciesMatch>>;
