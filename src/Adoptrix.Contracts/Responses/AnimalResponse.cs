namespace Adoptrix.Contracts.Responses;

public class AnimalResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required string Species { get; init; }
    public required string Breed { get; init; }
    public required string Sex { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required string Slug { get; init; }
    public required string Age { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required IEnumerable<AnimalImageResponse> Images { get; init; }
}
