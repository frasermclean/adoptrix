namespace Adoptrix.Application.Models;

public class ImageResult
{
    public required Guid Id { get; init; }
    public required string? Description { get; init; }
    public required bool IsProcessed { get; init; }
}
