using Adoptrix.Domain.Models;

namespace Adoptrix.Tests.Shared.Factories;

public static class AnimalImageFactory
{
    public static AnimalImage Create(Guid? id = null, string? description = null, string originalFileName = "file.jpg",
        string originalContentType = "image/jpeg", bool isProcessed = false, Guid? uploadedBy = null,
        DateTime? uploadedAt = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Description = description,
        OriginalFileName = originalFileName,
        OriginalContentType = originalContentType,
        IsProcessed = isProcessed,
        UploadedBy = uploadedBy ?? Guid.NewGuid(),
        UploadedAt = uploadedAt ?? DateTime.UtcNow,
        AnimalId = Guid.NewGuid()
    };

    public static IEnumerable<AnimalImage> CreateMany(int count) => Enumerable.Range(0, count).Select(_ => Create());
}
