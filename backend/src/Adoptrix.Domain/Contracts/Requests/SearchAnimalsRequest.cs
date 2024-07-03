using Adoptrix.Domain.Models;

namespace Adoptrix.Domain.Contracts.Requests;

public class SearchAnimalsRequest
{
    public string? Name { get; init; }
    public Guid? BreedId { get; init; }
    public Guid? SpeciesId { get; init; }
    public Sex? Sex { get; init; }
    public int? Limit { get; init; }
}
