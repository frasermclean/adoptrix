using Adoptrix.Application.Services;
using MediatR;
using SpeciesModel = Adoptrix.Domain.Models.Species;

namespace Adoptrix.Application.Features.Species.Queries;

public class GetAllSpeciesQueryHandler(ISpeciesRepository speciesRepository)
    : IRequestHandler<GetAllSpeciesQuery, IEnumerable<SpeciesModel>>
{
    public Task<IEnumerable<SpeciesModel>> Handle(GetAllSpeciesQuery query, CancellationToken cancellationToken)
    {
        return speciesRepository.GetAllAsync(cancellationToken);
    }
}
