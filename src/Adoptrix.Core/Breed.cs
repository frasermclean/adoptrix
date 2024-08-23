namespace Adoptrix.Core;

public class Breed : IUserCreatedEntity
{
    public const int NameMaxLength = 30;

    public int Id { get; init; }
    public required string Name { get; set; }
    public required Species Species { get; set; }
    public List<Animal> Animals { get; } = [];
    public Guid LastModifiedBy { get; set; }
    public DateTime LastModifiedUtc { get; set; }
}
