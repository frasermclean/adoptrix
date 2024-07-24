using Adoptrix.Contracts.Requests;
using Microsoft.AspNetCore.Http.Extensions;

namespace Adoptrix.Web.Extensions;

public static class SearchSpeciesQueryExtensions
{
    public static string ToQueryString(this SearchSpeciesRequest request)
    {
        var builder = new QueryBuilder();

        if (request.WithAnimals.HasValue && request.WithAnimals.Value)
        {
            builder.Add("withAnimals", "true");
        }

        return builder.ToString();
    }
}
