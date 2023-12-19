using Adoptrix.Domain;

namespace Adoptrix.Application.Models;

public class SearchAnimalsResult
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required string Species { get; init; }
    public required string? Breed { get; init; }
    public required Sex? Sex { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required ImageInformation? PrimaryImage { get; init; }

    public static SearchAnimalsResult FromAnimal(Animal animal) => new()
    {
        Id = animal.Id,
        Name = animal.Name,
        Description = animal.Description,
        Species = animal.Species.Name,
        Breed = animal.Breed?.Name,
        Sex = animal.Sex,
        DateOfBirth = animal.DateOfBirth,
        CreatedAt = animal.CreatedAt,
        PrimaryImage = animal.Images.Count > 0 ? animal.Images[0] : null
    };
}