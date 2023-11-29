using Adoptrix.Domain;
using FastEndpoints;

namespace Adoptrix.Application.Commands;

public class SearchAnimalsCommand : ICommand<IEnumerable<Animal>>
{
    public string? Name { get; init; }
    public string? Species { get; init; }
}