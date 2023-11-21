using Adoptrix.Domain;

namespace Adoptrix.Api.Endpoints.Animals.Update;

public class UpdateAnimalRequest
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required Species Species { get; init; }
    public required DateOnly DateOfBirth { get; init; }
}