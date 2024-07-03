using Adoptrix.Domain.Contracts.Requests.Breeds;
using Microsoft.AspNetCore.Http.Extensions;

namespace Adoptrix.Client.Extensions;

public static class SearchBreedsQueryExtensions
{
    public static string ToQueryString(this SearchBreedsRequest request)
    {
        var builder = new QueryBuilder();

        if (request.SpeciesId.HasValue)
        {
            builder.Add("speciesId", request.SpeciesId.ToString());
        }

        if (request.WithAnimals.HasValue)
        {
            builder.Add("withAnimals", "true");
        }

        return builder.ToString();
    }

}
