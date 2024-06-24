using Adoptrix.Domain.Models.Responses;
using MediatR;

namespace Adoptrix.Domain.Queries.Species;

public record SearchSpeciesQuery(bool WithAnimals = false) : IRequest<IEnumerable<SpeciesMatch>>;
