namespace Adoptrix.Core.Factories;

public static class SpeciesFactory
{
    public static Species Create(string name) => new()
    {
        Name = name
    };
}
