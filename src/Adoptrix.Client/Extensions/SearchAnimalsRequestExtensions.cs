using Adoptrix.Contracts.Requests;

namespace Adoptrix.Client.Extensions;

public static class SearchAnimalsRequestExtensions
{
    public static string ToQueryString(this SearchAnimalsRequest request)
    {
        var dictionary = request.ToDictionary();
        return dictionary.ToQueryString();
    }

    private static Dictionary<string, string?> ToDictionary(this SearchAnimalsRequest request) => new()
    {
        { "name", request.Name },
        { "breedId", request.BreedId?.ToString() },
        { "speciesId", request.SpeciesId?.ToString() },
        { "sex", request.Sex },
        { "limit", request.Limit?.ToString()}
    };
}
