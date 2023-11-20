namespace Adoptrix.Domain;

public class Animal : Entity
{
    public const int NameMaxLength = 50;

    public required string Name { get; init; }
    public required Species Species { get; init; }
    public DateOnly DateOfBirth { get; init; }
}