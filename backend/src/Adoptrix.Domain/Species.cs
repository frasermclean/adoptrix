namespace Adoptrix.Domain;

public class Species : AggregateRoot
{
    public const int NameMaxLength = 20;
    
    public string Name { get; init; } = string.Empty;
}