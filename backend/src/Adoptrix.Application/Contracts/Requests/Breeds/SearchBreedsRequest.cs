using Adoptrix.Application.Models;
using MediatR;

namespace Adoptrix.Application.Contracts.Requests.Breeds;

public class SearchBreedsRequest : IRequest<IEnumerable<SearchBreedsResult>>
{
    public Guid? SpeciesId { get; init; }
    public bool? WithAnimals { get; init; }
}
