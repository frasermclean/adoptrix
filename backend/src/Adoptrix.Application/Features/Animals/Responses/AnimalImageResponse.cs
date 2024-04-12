namespace Adoptrix.Application.Features.Animals.Responses;

public class AnimalImageResponse
{
    public required Guid Id { get; init; }
    public required string? Description { get; init; }
    public required bool IsProcessed { get; init; }
}
