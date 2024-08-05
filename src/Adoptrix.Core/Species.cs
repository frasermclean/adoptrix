namespace Adoptrix.Core;

public class Species : IUserCreatedEntity
{
    public const int NameMaxLength = 20;

    public required string Name { get; init; }
    public List<Breed> Breeds { get; } = [];
    public Guid CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }
}
