using Adoptrix.Domain.Models;
using Adoptrix.Domain.Models.Responses;
using MediatR;

namespace Adoptrix.Domain.Queries.Animals;

public record SearchAnimalsQuery(
    string? Name = null,
    Guid? BreedId = null,
    Guid? SpeciesId = null,
    Sex? Sex = null,
    int? Limit = null) : IRequest<IEnumerable<AnimalMatch>>;
