namespace Adoptrix.Core;

public class AnimalImage : IUserCreatedEntity
{
    public const int ContentTypeMaxLength = 50;

    public int Id { get; init; }
    public string? Description { get; init; }
    public required string OriginalFileName { get; init; }
    public required string OriginalContentType { get; init; }
    public bool IsProcessed { get; set; }
    public Guid CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }
}
