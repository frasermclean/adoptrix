using Adoptrix.Domain.Queries.Animals;
using Microsoft.AspNetCore.Http.Extensions;

namespace Adoptrix.Client.Extensions;

public static class SearchAnimalsQueryExtensions
{
    public static string ToQueryString(this SearchAnimalsQuery query)
    {
        var builder = new QueryBuilder();

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            builder.Add("name", query.Name);
        }

        if (query.BreedId.HasValue)
        {
            builder.Add("breedId", query.BreedId.ToString());
        }

        if (query.SpeciesId.HasValue)
        {
            builder.Add("speciesId", query.SpeciesId.ToString());
        }

        if (query.Sex.HasValue)
        {
            builder.Add("sex", query.Sex.ToString());
        }

        if (query.Limit.HasValue)
        {
            builder.Add("limit", query.Limit.ToString());
        }

        return builder.ToString();
    }
}
