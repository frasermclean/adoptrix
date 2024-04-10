using MediatR;
using SpeciesModel = Adoptrix.Domain.Models.Species;

namespace Adoptrix.Application.Contracts.Requests.Species;

public record GetAllSpeciesRequest : IRequest<IEnumerable<SpeciesModel>>;
