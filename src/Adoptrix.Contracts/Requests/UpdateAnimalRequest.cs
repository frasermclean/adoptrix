namespace Adoptrix.Contracts.Requests;

public class UpdateAnimalRequest
{
    public int AnimalId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public int BreedId { get; init; }
    public required string Sex { get; init; }
    public DateOnly DateOfBirth { get; init; }
}
