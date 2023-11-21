namespace Adoptrix.Domain;

public class Animal : AggregateRoot
{
    public const int NameMaxLength = 50;

    public required string Name { get; set; }
    public required Species Species { get; set; }
    public required DateOnly DateOfBirth { get; set; }
}