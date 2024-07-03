using Adoptrix.Application.Extensions;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Animals;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Queries;

public class SearchAnimalsQueryHandler(IAnimalsRepository animalsRepository, IAnimalImageManager animalImageManager)
    : IRequestHandler<SearchAnimalsQuery, IEnumerable<AnimalMatch>>
{
    public async Task<IEnumerable<AnimalMatch>> Handle(SearchAnimalsQuery query,
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
