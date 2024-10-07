namespace Adoptrix.Core;

public class Animal : IUserCreatedEntity
{
    public const int NameMaxLength = 30;
    public const int DescriptionMaxLength = 2000;
    public const int SlugMaxLength = 50;

    public Guid Id { get; init; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required Breed Breed { get; set; }
    public required Sex Sex { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required string Slug { get; init; }
    public List<AnimalImage> Images { get; init; } = [];
    public Guid LastModifiedBy { get; set; }
    public DateTime LastModifiedUtc { get; set; }

    public override bool Equals(object? otherObject)
        => otherObject is Animal otherAnimal && Id == otherAnimal.Id;

    public override int GetHashCode()
        => Id.GetHashCode();

    public static string CreateSlug(string name, DateOnly dateOfBirth)
    {
        name = string.Join('-', name.Trim().Split(' '));
        return $"{name.ToLowerInvariant()}-{dateOfBirth:O}";
    }
}
