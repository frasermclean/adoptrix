using Adoptrix.Application.Models;
using FastEndpoints;

namespace Adoptrix.Application.Commands.Animals;

public class SearchAnimalsCommand : ICommand<IEnumerable<SearchAnimalsResult>>
{
    public string? Name { get; init; }
    public string? Species { get; init; }
}