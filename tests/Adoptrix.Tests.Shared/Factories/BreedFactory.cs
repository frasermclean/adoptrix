using Adoptrix.Core;

namespace Adoptrix.Tests.Shared.Factories;

public static class BreedFactory
{
    private static readonly string[] Names = ["Golden Retriever", "German Shepherd", "Border Collie"];

    public static Breed Create(int? id = null, string? name = null, Species? species = null, Guid? createdBy = null) =>
        new()
        {
            Id = id ?? default,
            Name = name ?? Names[Random.Shared.Next(Names.Length)],
            Species = species ?? SpeciesFactory.Create(),
            CreatedBy = createdBy ?? Guid.Empty
        };

    public static IEnumerable<Breed> CreateMany(int count) => Enumerable.Range(0, count).Select(_ => Create());
}
