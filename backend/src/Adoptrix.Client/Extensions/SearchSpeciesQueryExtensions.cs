using Adoptrix.Domain.Queries.Species;
using Microsoft.AspNetCore.Http.Extensions;

namespace Adoptrix.Client.Extensions;

public static class SearchSpeciesQueryExtensions
{
    public static string ToQueryString(this SearchSpeciesQuery query)
    {
        var builder = new QueryBuilder();

        if (query.WithAnimals)
        {
            builder.Add("withAnimals", "true");
        }

        return builder.ToString();
    }
}
