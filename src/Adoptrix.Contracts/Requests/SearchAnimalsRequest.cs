namespace Adoptrix.Contracts.Requests;

public class SearchAnimalsRequest
{
    public string? Name { get; init; }
    public Guid? BreedId { get; init; }
    public Guid? SpeciesId { get; init; }
    public string? Sex { get; init; }
    public int? Limit { get; init; }
}
