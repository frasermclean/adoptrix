namespace Adoptrix.Application.Models;

public class SearchBreedsResult
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Species { get; init; }
    public required IEnumerable<int> AnimalIds { get; init; }
}