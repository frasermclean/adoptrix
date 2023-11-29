using Adoptrix.Domain;
using FastEndpoints;

namespace Adoptrix.Application.Commands;

public class SearchAnimalsCommand : ICommand<IEnumerable<Animal>>
{
    public string? AnimalName { get; init; }
    public string? SpeciesName { get; init; }
}