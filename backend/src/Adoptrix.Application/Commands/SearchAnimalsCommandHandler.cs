using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using FastEndpoints;

namespace Adoptrix.Application.Commands;

public class SearchAnimalsCommandHandler(IAnimalsRepository repository, IAnimalImageManager imageManager)
    : ICommandHandler<SearchAnimalsCommand, IEnumerable<AnimalSearchResult>>
{
    public async Task<IEnumerable<AnimalSearchResult>> ExecuteAsync(SearchAnimalsCommand command,
        CancellationToken cancellationToken)
    {
        var animals = await repository.SearchAsync(command.Name, command.Species, cancellationToken);

        return animals.Select(animal => new AnimalSearchResult(animal)
        {
            MainImage = animal.Images.Count > 0
                ? new AnimalImageResult
                {
                    Uri = imageManager.GetImageUri(animal.Id, animal.Images[0].FileName),
                    Description = animal.Images[0].Description
                }
                : null
        });
    }
}