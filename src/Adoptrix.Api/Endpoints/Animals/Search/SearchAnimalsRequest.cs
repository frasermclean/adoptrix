using Adoptrix.Domain;

namespace Adoptrix.Api.Endpoints.Animals.Search;

public class SearchAnimalsRequest
{
    public string? Name { get; set; }
    public Species? Species { get; set; }
}