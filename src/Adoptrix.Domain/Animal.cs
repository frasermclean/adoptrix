namespace Adoptrix.Domain;

public class Animal : Entity
{
    public const int NameMaxLength = 50;

    public required string Name { get; set; }
    public required Species Species { get; set; }
    public DateOnly DateOfBirth { get; set; }
}