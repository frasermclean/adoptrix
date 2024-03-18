using Adoptrix.Domain;

namespace Adoptrix.Application.Models;

public class SearchAnimalsResult
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required Guid SpeciesId { get; init; }
    public required Guid BreedId { get; init; }
    public required Sex Sex { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required IEnumerable<ImageInformation> Images { get; init; }
}
