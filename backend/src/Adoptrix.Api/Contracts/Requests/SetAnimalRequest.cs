using Adoptrix.Domain.Models;

namespace Adoptrix.Api.Contracts.Requests;

public class SetAnimalRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required Guid SpeciesId { get; init; }
    public required Guid BreedId { get; init; }
    public Sex Sex { get; init; }
    public required DateOnly DateOfBirth { get; init; }

    public override string ToString() => $"{Name} {DateOfBirth}";
}
