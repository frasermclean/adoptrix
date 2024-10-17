namespace Adoptrix.Core;

public class Breed : ILastModifiedEntity
{
    public const int NameMaxLength = 30;
    private const string DefaultName = "Unknown";

    private Breed()
    {
    }

    public int Id { get; private init; }
    public required string Name { get; set; }
    public required Species Species { get; set; }
    public List<Animal> Animals { get; } = [];
    public Guid? LastModifiedBy { get; set; }
    public DateTime LastModifiedUtc { get; set; }

    public static Breed Create(string name = DefaultName, Species? species = null)
    {
        if (name.Length > NameMaxLength)
        {
            throw new ArgumentException($"Name cannot exceed {NameMaxLength} characters.", nameof(name));
        }

        return new Breed
        {
            Id = default,
            Name = name,
            Species = species ?? Species.Create()
        };
    }
}
