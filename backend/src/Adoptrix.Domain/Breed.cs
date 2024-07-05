namespace Adoptrix.Domain;

public class Breed : Aggregate
{
    public const int NameMaxLength = 30;

    public required string Name { get; set; }
    public required Species Species { get; set; }
    public ICollection<Animal> Animals { get; } = new List<Animal>();
}