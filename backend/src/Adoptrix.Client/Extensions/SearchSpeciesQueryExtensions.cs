using Adoptrix.Domain.Contracts.Requests.Species;
using Microsoft.AspNetCore.Http.Extensions;

namespace Adoptrix.Client.Extensions;

public static class SearchSpeciesQueryExtensions
{
    public static string ToQueryString(this SearchSpeciesRequest request)
    {
        var builder = new QueryBuilder();

        if (request.WithAnimals)
        {
            builder.Add("withAnimals", "true");
        }

        return builder.ToString();
    }
}
