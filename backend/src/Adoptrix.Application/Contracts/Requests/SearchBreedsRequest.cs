namespace Adoptrix.Application.Contracts.Requests;

public class SearchBreedsRequest
{
    public Guid? SpeciesId { get; init; }
    public bool? WithAnimals { get; init; }
}
