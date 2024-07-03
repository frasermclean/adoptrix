namespace Adoptrix.Domain.Contracts.Responses;

public class AnimalImageResponse
{
    public required Guid Id { get; init; }
    public required string? Description { get; init; }
    public required bool IsProcessed { get; init; }
    public string? PreviewUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? FullSizeUrl { get; set; }
}
