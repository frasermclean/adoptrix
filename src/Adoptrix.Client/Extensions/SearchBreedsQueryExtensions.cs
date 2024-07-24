using Adoptrix.Contracts.Requests;

namespace Adoptrix.Client.Extensions;

public static class SearchBreedsQueryExtensions
{
    public static string ToQueryString(this SearchBreedsRequest request)
    {
        var dictionary = request.ToDictionary();
        return dictionary.ToQueryString();
    }

    private static Dictionary<string, string?> ToDictionary(this SearchBreedsRequest request) => new()
    {
        { "speciesId", request.SpeciesId?.ToString() },
        { "withAnimals", request.WithAnimals?.ToString() }
    };

}
