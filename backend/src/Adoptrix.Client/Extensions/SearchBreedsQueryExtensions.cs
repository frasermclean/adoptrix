using Adoptrix.Domain.Queries.Breeds;
using Microsoft.AspNetCore.Http.Extensions;

namespace Adoptrix.Client.Extensions;

public static class SearchBreedsQueryExtensions
{
    public static string ToQueryString(this SearchBreedsQuery query)
    {
        var builder = new QueryBuilder();

        if (query.SpeciesId.HasValue)
        {
            builder.Add("speciesId", query.SpeciesId.ToString());
        }

        if (query.WithAnimals.HasValue)
        {
            builder.Add("withAnimals", "true");
        }

        return builder.ToString();
    }

}
