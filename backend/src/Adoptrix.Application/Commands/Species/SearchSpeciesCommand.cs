using FastEndpoints;

namespace Adoptrix.Application.Commands.Species;

public class SearchSpeciesCommand : ICommand<IEnumerable<Domain.Species>>;