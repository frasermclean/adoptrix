using Adoptrix.Application.Models;
using Adoptrix.Domain;
using FastEndpoints;

namespace Adoptrix.Application.Commands;

public class SearchAnimalsCommand : ICommand<IEnumerable<AnimalSearchResult>>
{
    public string? Name { get; init; }
    public Species? Species { get; init; }
}