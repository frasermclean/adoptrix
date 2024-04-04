namespace Adoptrix.Domain.Models.Factories;

public static class BreedFactory
{
    private static readonly string[] Names = ["Golden Retriever", "German Shepherd", "Border Collie"];

    public static Breed Create(Guid? id = null, string? name = null, Species? species = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = name ?? Names[Random.Shared.Next(Names.Length)],
        Species = species ?? SpeciesFactory.Create()
    };

    public static IEnumerable<Breed> CreateMany(int count) => Enumerable.Range(0, count).Select(_ => Create());
}
