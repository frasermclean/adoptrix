using Adoptrix.Core.Contracts.Requests.Animals;
using Microsoft.AspNetCore.Http.Extensions;

namespace Adoptrix.Web.Extensions;

public static class SearchAnimalsRequestExtensions
{
    public static string ToQueryString(this SearchAnimalsRequest request)
    {
        var builder = new QueryBuilder();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            builder.Add("name", request.Name);
        }

        if (request.BreedId.HasValue)
        {
            builder.Add("breedId", request.BreedId.ToString());
        }

        if (request.SpeciesId.HasValue)
        {
            builder.Add("speciesId", request.SpeciesId.ToString());
        }

        if (request.Sex.HasValue)
        {
            builder.Add("sex", request.Sex.ToString());
        }

        if (request.Limit.HasValue)
        {
            builder.Add("limit", request.Limit.ToString());
        }

        return builder.ToString();
    }
}
