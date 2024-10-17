namespace Adoptrix.Core;

public class Breed : ILastModifiedEntity
{
    public const int NameMaxLength = 30;
    private const string DefaultName = "Unknown";

    public Breed(string name = DefaultName)
    {
        if (name.Length > NameMaxLength)
        {
            throw new ArgumentException($"Name cannot exceed {NameMaxLength} characters.", nameof(name));
        }

        Id = default;
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
    public required Species Species { get; set; }
    public List<Animal> Animals { get; } = [];
    public Guid? LastModifiedBy { get; set; }
    public DateTime LastModifiedUtc { get; set; }
}
