namespace Adoptrix.Domain.Models.Factories;

public static class BreedFactory
{
    public static Breed CreateBreed(Guid? id = null, string name = "Golden Retriever", Species? species = null) => new()
    {
        Id = id ?? Guid.NewGuid(), Name = name, Species = species ?? SpeciesFactory.CreateSpecies()
    };
}
