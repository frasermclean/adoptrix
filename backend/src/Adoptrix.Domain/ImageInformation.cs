﻿namespace Adoptrix.Domain;

public class ImageInformation : Entity
{
    public const int ContentTypeMaxLength = 50;

    public string? Description { get; init; }
    public required string OriginalFileName { get; init; }
    public required string OriginalContentType { get; init; }
    public bool HasThumbnail { get; set; }
    public bool HasPreview { get; set; }
    public bool HasFullSize { get; set; }

    public required Guid? UploadedBy { get; init; }
    public DateTime UploadedAt { get; init; }
}