using Adoptrix.Application.Models;
using Adoptrix.Domain.Models;
using MediatR;

namespace Adoptrix.Application.Contracts.Requests.Animals;

public record SearchAnimalsRequest(
    string? Name,
    Guid? BreedId,
    Guid? SpeciesId,
    Sex? Sex,
    int? Limit) : IRequest<IEnumerable<SearchAnimalsResult>>;
