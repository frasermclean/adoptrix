namespace Adoptrix.Core;

public class AnimalImage : IUserCreatedEntity
{
    public const int ContentTypeMaxLength = 50;

    public Guid Id { get; init; }
    public required string AnimalSlug { get; init; }
    public string? Description { get; init; }
    public required string OriginalFileName { get; init; }
    public required string OriginalContentType { get; init; }
    public bool IsProcessed { get; set; }
    public Guid CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }

    public string GetOriginalBlobName() => $"{AnimalSlug}/{OriginalFileName}";
}
