using Adoptrix.Domain;

namespace Adoptrix.Api.Endpoints.Animals.Search;

public class SearchAnimalsResponse
{
    public IEnumerable<Animal> Animals { get; init; } = Enumerable.Empty<Animal>();
}