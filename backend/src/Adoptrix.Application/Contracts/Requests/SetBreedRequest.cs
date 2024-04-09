namespace Adoptrix.Application.Contracts.Requests;

public sealed class SetBreedRequest
{
    public required string Name { get; init; }
    public required Guid SpeciesId { get; init; }
    public Guid UserId { get; set; }

    public override string ToString() => $"Name: {Name}, SpeciesId: {SpeciesId}";
}
