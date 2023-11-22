namespace Adoptrix.Domain;

public class Animal : AggregateRoot
{
    public const int NameMaxLength = 50;
    public const int DescriptionMaxLength = 2000;

    public required string Name { get; set; }
    public string? Description { get; set; }
    public required Species Species { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public List<ImageInformation> Images { get; set; } = new();
}