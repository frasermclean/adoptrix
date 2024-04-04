namespace Adoptrix.Domain.Factories;

public static class AnimalImageFactory
{
    public static AnimalImage CreateAnimalImage(Guid? id = null, string? description = null,
        string originalFileName = "file.jpg", string originalContentType = "image/jpeg",
        bool isProcessed = false, Guid? uploadedBy = null, DateTime? uploadedAt = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Description = description,
        OriginalFileName = originalFileName,
        OriginalContentType = originalContentType,
        IsProcessed = isProcessed,
        UploadedBy = uploadedBy ?? Guid.NewGuid(),
        UploadedAt = uploadedAt ?? DateTime.UtcNow
    };
}
