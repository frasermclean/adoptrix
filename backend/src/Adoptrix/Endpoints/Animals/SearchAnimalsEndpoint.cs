using Adoptrix.Application.Extensions;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Animals;
using FastEndpoints;

namespace Adoptrix.Endpoints.Animals;

public class SearchAnimalsEndpoint(IAnimalsRepository animalsRepository, IAnimalImageManager animalImageManager)
    : Endpoint<SearchAnimalsQuery, IEnumerable<AnimalMatch>>
{
    public override void Configure()
    {
        Get("animals");
        AllowAnonymous();
    }

    public override async Task<IEnumerable<AnimalMatch>> ExecuteAsync(SearchAnimalsQuery query,
        CancellationToken cancellationToken)
    {
        var matches = await animalsRepository.SearchAsync(query, cancellationToken);
        var matchesList = matches as List<AnimalMatch> ?? matches.ToList();

        foreach (var match in matchesList)
        {
            match.Image?.SetImageUrls(match.Id, animalImageManager.ContainerUri);
        }

        return matchesList;
    }
}
