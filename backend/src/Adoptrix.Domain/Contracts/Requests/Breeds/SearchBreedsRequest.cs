namespace Adoptrix.Domain.Contracts.Requests.Breeds;

public class SearchBreedsRequest
{
    public Guid? SpeciesId { get; init; }
    public bool? WithAnimals { get; init; }
}
