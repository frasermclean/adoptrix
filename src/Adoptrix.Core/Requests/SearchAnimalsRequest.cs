namespace Adoptrix.Core.Requests;

public class SearchAnimalsRequest
{
    public string? Name { get; init; }
    public int? BreedId { get; init; }
    public string? SpeciesName { get; init; }
    public Sex? Sex { get; init; }
    public int? Limit { get; init; }
}
