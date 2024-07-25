namespace Adoptrix.Client.Extensions;

public static class DictionaryExtensions
{
    public static string ToQueryString(this Dictionary<string, string?> dictionary)
    {
        var queryString = string.Join('&', dictionary
            .Where(pair => pair.Value != null)
            .Select(pair => $"{pair.Key}={pair.Value}"));

        return queryString.Length > 0 ? $"?{queryString}" : "";
    }
}
