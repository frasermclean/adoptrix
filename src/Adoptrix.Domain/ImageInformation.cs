namespace Adoptrix.Domain;

public class ImageInformation
{
    public const int FileNameMaxLength = 40;
    public const int ContentTypeMaxLength = 50;

    public int Id { get; init; }
    public required string FileName { get; init; }
    public string? Description { get; init; }
    public required string OriginalFileName { get; init; }
    public required Guid? UploadedBy { get; init; }
    public DateTime UploadedAt { get; init; }
}