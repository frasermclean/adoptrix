using Adoptrix.Core;

namespace Adoptrix.Api.Endpoints.Animals;

public class UpdateAnimalRequest
{
    public Guid AnimalId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public int BreedId { get; init; }
    public Sex Sex { get; init; }
    public DateOnly DateOfBirth { get; init; }
}
