using Adoptrix.Application.Models;
using FastEndpoints;

namespace Adoptrix.Application.Commands.Breeds;

public sealed class SearchBreedsCommand : ICommand<IEnumerable<SearchBreedsResult>>
{
    public string? Species { get; init; }
    public bool WithAnimals { get; init; }
}