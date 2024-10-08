namespace Adoptrix.Core.Responses;

public class AnimalImageResponse
{
    public required Guid Id { get; init; }
    public required string? Description { get; init; }
    public string? PreviewUrl { get; init; }
    public string? ThumbnailUrl { get; init; }
    public string? FullSizeUrl { get; init; }
}
