using FastEndpoints;

namespace Adoptrix.Application.Commands.Species;

public class GetAllSpeciesCommand : ICommand<IEnumerable<Domain.Species>>;