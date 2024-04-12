using Adoptrix.Application.Features.Breeds.Responses;
using MediatR;

namespace Adoptrix.Application.Features.Breeds.Queries;

public class SearchBreedsQuery : IRequest<IEnumerable<SearchBreedsResult>>
{
    public Guid? SpeciesId { get; init; }
    public bool? WithAnimals { get; init; }
}
