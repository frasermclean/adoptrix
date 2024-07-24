namespace Adoptrix.Contracts.Responses;

public class AnimalMatch
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string SpeciesName { get; init; }
    public required string BreedName { get; init; }
    public AnimalImageResponse? Image { get; init; }
}
