namespace Adoptrix.Contracts.Requests;

public class SearchAnimalsRequest
{
    public string? Name { get; init; }
    public int? BreedId { get; init; }
    public string? SpeciesName { get; init; }
    public string? Sex { get; init; }
    public int? Limit { get; init; }
}
