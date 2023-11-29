namespace Adoptrix.Domain;

public class Species : AggregateRoot
{
    public string Name { get; init; } = string.Empty;
}