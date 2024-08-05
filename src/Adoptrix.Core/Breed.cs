namespace Adoptrix.Core;

public class Breed
{
    public const int NameMaxLength = 30;

    public int Id { get; init; }
    public required string Name { get; set; }
    public required Species Species { get; set; }
    public List<Animal> Animals { get; } = [];
    public Guid? CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }
}
