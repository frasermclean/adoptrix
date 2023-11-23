using Adoptrix.Domain;

namespace Adoptrix.Application.Models;

public class AnimalSearchResult
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required Species Species { get; init; }

    public static AnimalSearchResult FromAnimal(Animal animal) => new()
    {
        Id = animal.Id,
        Name = animal.Name,
        Description = animal.Description,
        Species = animal.Species
    };
}