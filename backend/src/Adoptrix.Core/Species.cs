namespace Adoptrix.Core;

public class Species : ILastModifiedEntity
{
    public const int NameMaxLength = 20;

    public int Id { get; init; }
    public required string Name { get; init; }
    public List<Breed> Breeds { get; } = [];
    public Guid LastModifiedBy { get; set; }
    public DateTime LastModifiedUtc { get; set; }
}
