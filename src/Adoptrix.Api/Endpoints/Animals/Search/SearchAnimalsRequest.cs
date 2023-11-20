using Adoptrix.Domain;

namespace Adoptrix.Api.Endpoints.Animals.Search;

public class SearchAnimalsRequest
{
    public Species? Species { get; init; }
    public string? Name { get; init; }
}