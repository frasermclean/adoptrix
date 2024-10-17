namespace Adoptrix.Core;

public class AnimalImage : ILastModifiedEntity
{
    public const int ContentTypeMaxLength = 50;

    private AnimalImage()
    {
    }

    public Guid Id { get; private init; }
    public required string AnimalSlug { get; init; }
    public string? Description { get; private init; }
    public required string OriginalFileName { get; init; }
    public required string OriginalContentType { get; init; }
    public bool IsProcessed { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime LastModifiedUtc { get; set; }

    public string OriginalBlobName => $"{AnimalSlug}/{OriginalFileName}";
    public string PreviewBlobName => $"{AnimalSlug}/{Id}/preview.webp";
    public string ThumbnailBlobName => $"{AnimalSlug}/{Id}/thumb.webp";
    public string FullSizeBlobName => $"{AnimalSlug}/{Id}/full.webp";

    public static AnimalImage Create(string animalSlug, string? description, string originalFileName,
        string originalContentType) => new()
    {
        Id = default,
        AnimalSlug = animalSlug,
        Description = description,
        OriginalFileName = originalFileName,
        OriginalContentType = originalContentType
    };
}
