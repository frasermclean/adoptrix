namespace Adoptrix.Contracts.Responses;

public class AnimalMatch
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string SpeciesName { get; init; }
    public required string BreedName { get; init; }
    public required AnimalImageResponse Image { get; init; }
}
