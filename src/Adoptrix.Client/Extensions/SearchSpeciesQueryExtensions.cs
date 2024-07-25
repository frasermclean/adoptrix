using Adoptrix.Contracts.Requests;

namespace Adoptrix.Client.Extensions;

public static class SearchSpeciesQueryExtensions
{
    public static string ToQueryString(this SearchSpeciesRequest request)
    {
        var dictionary = request.ToDictionary();
        return dictionary.ToQueryString();
    }

    private static Dictionary<string, string?> ToDictionary(this SearchSpeciesRequest request) => new()
    {
        { "withAnimals", request.WithAnimals?.ToString() }
    };
}
