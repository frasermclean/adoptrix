using Adoptrix.Core;

namespace Adoptrix.Api.Endpoints.Animals;

public class AddAnimalRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required string BreedName { get; init; }
    public required Sex Sex { get; init; }
    public DateOnly DateOfBirth { get; init; }
}
