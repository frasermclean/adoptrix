using Adoptrix.Core;

namespace Adoptrix.Tests.Shared.Factories;

public static class AnimalImageFactory
{
    public static AnimalImage Create(int? id = null, string? description = null, string originalFileName = "file.jpg",
        string originalContentType = "image/jpeg", bool isProcessed = false, Guid? uploadedBy = null,
        DateTime? uploadedAt = null) => new()
    {
        Id = id ?? default,
        Description = description,
        OriginalFileName = originalFileName,
        OriginalContentType = originalContentType,
        IsProcessed = isProcessed,
        CreatedBy = uploadedBy ?? Guid.NewGuid(),
        CreatedAt = uploadedAt ?? DateTime.UtcNow
    };

    public static IEnumerable<AnimalImage> CreateMany(int count) => Enumerable.Range(0, count).Select(_ => Create());
}
