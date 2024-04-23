using Adoptrix.Application.Features.Animals.Responses;
using Adoptrix.Domain.Models;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Queries;

public record SearchAnimalsQuery(
    string? Name,
    Guid? BreedId,
    Guid? SpeciesId,
    Sex? Sex,
    int? Limit) : IRequest<IEnumerable<SearchAnimalsMatch>>;
