namespace Adoptrix.Domain;

public abstract class Animal : Entity
{
    public required string Name { get; init; }
    public required Species Species { get; init; }
}