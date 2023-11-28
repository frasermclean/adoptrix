using Adoptrix.Domain;

namespace Adoptrix.Api.Endpoints.Animals.Update;

public class UpdateAnimalRequest
{
    public string Id { get; init; } = string.Empty;
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required Species Species { get; init; }
    public required DateOnly DateOfBirth { get; init; }
}