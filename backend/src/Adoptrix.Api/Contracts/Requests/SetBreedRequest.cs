namespace Adoptrix.Api.Contracts.Requests;

public sealed class SetBreedRequest
{
    public required string Name { get; init; }
    public required Guid SpeciesId { get; init; }

    public override string ToString() => $"Name: {Name}, SpeciesId: {SpeciesId}";
}
