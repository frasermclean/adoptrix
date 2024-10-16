namespace Adoptrix.Core.Factories;

public static class BreedFactory
{
    private static readonly string[] Names = ["Golden Retriever", "German Shepherd", "Border Collie"];

    public static Breed Create(int? id = null, string? name = null, Species? species = null,
        Guid? lastModifiedBy = null) => new()
    {
        Id = id ?? default,
        Name = name ?? Names[Random.Shared.Next(Names.Length)],
        Species = species ?? SpeciesFactory.Create(),
        LastModifiedBy = lastModifiedBy ?? Guid.Empty
    };
}
