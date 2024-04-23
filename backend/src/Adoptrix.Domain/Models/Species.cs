namespace Adoptrix.Domain.Models;

public class Species : Aggregate
{
    public const int NameMaxLength = 20;

    public string Name { get; init; } = string.Empty;
    public ICollection<Breed> Breeds { get; } = new List<Breed>();
}
