namespace Adoptrix.Core;

public class Species : IUserCreatedEntity
{
    public const int NameMaxLength = 20;

    public int Id { get; init; }
    public required string Name { get; init; }
    public List<Breed> Breeds { get; } = [];
    public Guid LastModifiedBy { get; init; }
    public DateTime LastModifiedUtc { get; init; }
}
