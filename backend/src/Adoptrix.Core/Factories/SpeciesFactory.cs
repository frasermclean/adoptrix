namespace Adoptrix.Core.Factories;

public static class SpeciesFactory
{
    private static readonly string[] Names = ["Dog", "Cat", "Bird"];

    public static Species Create(string? name = null) => new()
    {
        Name = name ?? Names[Random.Shared.Next(Names.Length)]
    };
}
