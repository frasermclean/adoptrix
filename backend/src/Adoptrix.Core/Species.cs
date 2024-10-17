namespace Adoptrix.Core;

public class Species : ILastModifiedEntity
{
    public const int NameMaxLength = 20;
    private const string DefaultName = "Unknown";

    private Species()
    {
    }

    public int Id { get; private init; }
    public required string Name { get; init; }
    public List<Breed> Breeds { get; } = [];
    public Guid? LastModifiedBy { get; set; }
    public DateTime LastModifiedUtc { get; set; }

    public static Species Create(string name = DefaultName)
    {
        if (name.Length > NameMaxLength)
        {
            throw new ArgumentException($"Name cannot exceed {NameMaxLength} characters.", nameof(name));
        }

        return new Species
        {
            Id = default,
            Name = name
        };
    }
}
