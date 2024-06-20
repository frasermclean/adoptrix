using Adoptrix.Domain.Models.Responses;
using MediatR;

namespace Adoptrix.Domain.Queries.Breeds;

public class SearchBreedsQuery : IRequest<IEnumerable<BreedMatch>>
{
    public Guid? SpeciesId { get; init; }
    public bool? WithAnimals { get; init; }
}
