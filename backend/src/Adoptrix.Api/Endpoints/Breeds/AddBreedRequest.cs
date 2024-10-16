namespace Adoptrix.Api.Endpoints.Breeds;

public class AddBreedRequest
{
    public required string Name { get; init; }
    public required string SpeciesName { get; init; }
}
