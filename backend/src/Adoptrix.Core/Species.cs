namespace Adoptrix.Core;

public class Species : ILastModifiedEntity
{
    public const int NameMaxLength = 20;
    private const string DefaultName = "Unknown";

    public Species(string name = DefaultName)
    {
        if (name.Length > NameMaxLength)
        {
            throw new ArgumentException($"Name cannot exceed {NameMaxLength} characters.", nameof(name));
        }

        Id = default;
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public List<Breed> Breeds { get; } = [];
    public Guid? LastModifiedBy { get; set; }
    public DateTime LastModifiedUtc { get; set; }
}
