namespace Adoptrix.Api.Contracts.Responses;

public class AnimalImageResponse
{
    public required Guid Id { get; init; }
    public required string? Description { get; init; }
    public required bool HasThumbnail { get; init; }
    public required bool HasPreview { get; init; }
    public required bool HasFullSize { get; init; }
}