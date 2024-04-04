namespace Adoptrix.Domain.Models;

public class AnimalImage : Entity
{
    public const int ContentTypeMaxLength = 50;

    public string? Description { get; init; }
    public required string OriginalFileName { get; init; }
    public required string OriginalContentType { get; init; }
    public bool IsProcessed { get; set; }

    public required Guid? UploadedBy { get; init; }
    public DateTime UploadedAt { get; init; }
}
