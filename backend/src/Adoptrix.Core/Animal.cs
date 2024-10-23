using Humanizer;

namespace Adoptrix.Core;

public class Animal : ILastModified
{
    public const int NameMaxLength = 30;
    public const int DescriptionMaxLength = 2000;
    public const int SlugMaxLength = 50;

    public Animal(string name, DateOnly dateOfBirth, string? description = null)
    {
        if (name.Length > NameMaxLength)
        {
            throw new ArgumentException($"Name cannot exceed {NameMaxLength} characters.", nameof(name));
        }

        if (description?.Length > DescriptionMaxLength)
        {
            throw new ArgumentException($"Description cannot exceed {DescriptionMaxLength} characters.",
                nameof(description));
        }

        Name = name;
        Description = description;
        DateOfBirth = dateOfBirth;
        Slug = CreateSlug(name, dateOfBirth);
    }

    public Guid Id { get; init; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public required Breed Breed { get; set; }
    public required Sex Sex { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Slug { get; private set; }
    public List<AnimalImage> Images { get; } = [];
    public Guid? LastModifiedBy { get; set; }
    public DateTime LastModifiedUtc { get; set; }

    public static string CreateSlug(string name, DateOnly dateOfBirth)
        => $"{name.Kebaberize()}-{dateOfBirth:O}";

    public override bool Equals(object? otherObject)
        => otherObject is Animal otherAnimal && Id == otherAnimal.Id;

    public override int GetHashCode()
        => Id.GetHashCode();
}
