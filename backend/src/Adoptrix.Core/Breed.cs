namespace Adoptrix.Core;

public class Breed : ILastModifiedEntity
{
    public const int NameMaxLength = 30;

    public int Id { get; init; }
    public required string Name { get; set; }
    public required Species Species { get; set; }
    public List<Animal> Animals { get; } = [];
    public Guid LastModifiedBy { get; set; }
    public DateTime LastModifiedUtc { get; set; }

    public static Breed Create(string name, Species? species = null) => new()
    {
        Name = name,
        Species = species ?? Species.Create(Guid.NewGuid().ToString())
    };
}
