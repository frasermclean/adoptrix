namespace Adoptrix.Contracts.Responses;

public class AnimalImageResponse
{
    public required int Id { get; init; }
    public required string? Description { get; init; }
    public required bool IsProcessed { get; init; }
}
