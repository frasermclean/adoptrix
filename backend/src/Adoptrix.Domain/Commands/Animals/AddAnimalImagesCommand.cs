namespace Adoptrix.Domain.Commands.Animals;

public class AddAnimalImagesCommand
{
    public required Guid AnimalId { get; init; }
    public required Guid UserId { get; init; }
    public required IEnumerable<AnimalImageFileData> FileData { get; init; }
}

public class AnimalImageFileData
{
    public required string FileName { get; init; }
    public required string Description { get; init; }
    public required string ContentType { get; init; }
    public required long Length { get; init; }
    public required Stream Stream { get; init; }
}
