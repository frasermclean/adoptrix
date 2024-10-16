namespace Adoptrix.Core.Factories;

public static class BreedFactory
{
    public static Breed Create(string name, Species? species = null) => new()
    {
        Name = name,
        Species = species ?? SpeciesFactory.Create(Guid.NewGuid().ToString())
    };
}
