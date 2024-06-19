using Adoptrix.Application.Features.Animals.Responses;
using Adoptrix.Domain.Models;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Queries;

public record SearchAnimalsQuery(
    string? Name = null,
    Guid? BreedId = null,
    Guid? SpeciesId = null,
    Sex? Sex = null,
    int? Limit = null) : IRequest<IEnumerable<SearchAnimalsResult>>;
