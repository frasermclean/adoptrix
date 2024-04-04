namespace Adoptrix.Domain.Models.Factories;

public static class SpeciesFactory
{
    public static Species CreateSpecies(Guid? id = null, string name = "Dog") => new()
    {
        Id = id ?? Guid.NewGuid(), Name = name
    };
}
