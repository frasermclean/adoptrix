using Adoptrix.Domain.Models;

namespace Adoptrix.Application.Contracts.Requests.Animals;

public class SearchAnimalsRequest
{
    public string? Name { get; init; }
    public Guid? BreedId { get; init; }
    public Guid? SpeciesId { get; init; }
    public Sex? Sex { get; init; }
}
