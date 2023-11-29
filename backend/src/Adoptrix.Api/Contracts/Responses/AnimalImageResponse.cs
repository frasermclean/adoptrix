namespace Adoptrix.Api.Contracts.Responses;

public class AnimalImageResponse
{
    public required int Id { get; init; }
    public required Uri Uri { get; init; }
    public required string? Description { get; init; }
}