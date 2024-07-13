namespace Adoptrix.Core;

public class AnimalImage : Entity
{
    public const int ContentTypeMaxLength = 50;

    public string? Description { get; init; }
    public required string OriginalFileName { get; init; }
    public required string OriginalContentType { get; init; }
    public bool IsProcessed { get; set; }
    public required Guid? UploadedBy { get; init; }
    public DateTime UploadedAt { get; init; }
    public Guid AnimalId { get; init; }

    public string GetBlobName(AnimalImageCategory category) => GetBlobName(AnimalId, Id, category);

    public static string GetBlobName(Guid animalId, Guid imageId, AnimalImageCategory category)
    {
        var suffix = category switch
        {
            AnimalImageCategory.FullSize => "full",
            AnimalImageCategory.Thumbnail => "thumb",
            AnimalImageCategory.Preview => "preview",
            _ => "original"
        };

        return $"{animalId}/{imageId}/{suffix}";
    }
}
