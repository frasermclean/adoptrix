using Adoptrix.Application.Contracts.Requests.Species;
using Adoptrix.Application.Services.Repositories;
using MediatR;
using SpeciesModel = Adoptrix.Domain.Models.Species;

namespace Adoptrix.Application.Handlers.Species;

public class GetAllSpeciesHandler(ISpeciesRepository speciesRepository)
    : IRequestHandler<GetAllSpeciesRequest, IEnumerable<SpeciesModel>>
{
    public Task<IEnumerable<SpeciesModel>> Handle(GetAllSpeciesRequest request, CancellationToken cancellationToken)
    {
        return speciesRepository.GetAllAsync(cancellationToken);
    }
}
