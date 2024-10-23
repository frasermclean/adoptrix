namespace Adoptrix.Core;

public class Breed : ILastModified
{
    public const int NameMaxLength = 30;

    public Breed(string name)
    {
        if (name.Length > NameMaxLength)
        {
            throw new ArgumentException($"Name cannot exceed {NameMaxLength} characters.", nameof(name));
        }

        Name = name;
    }

    public int Id { get; init; }
    public string Name { get; set; }
    public required Species Species { get; set; }
    public List<Animal> Animals { get; } = [];
    public Guid? LastModifiedBy { get; set; }
    public DateTime LastModifiedUtc { get; set; }
}
