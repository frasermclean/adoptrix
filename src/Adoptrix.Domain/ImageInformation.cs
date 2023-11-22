namespace Adoptrix.Domain;

public class ImageInformation
{
    public const int FileNameMaxLength = 100;
    public const int ContentTypeMaxLength = 50;

    public required string FileName { get; init; }
    public string? Description { get; init; }
    public required string ContentType { get; init; }
    public required Guid UploadedBy { get; init; }
    public DateTime UploadedAt { get; init; }
}