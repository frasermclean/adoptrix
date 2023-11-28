using Adoptrix.Domain;

namespace Adoptrix.Application.Models;

public class AnimalSearchResult(Animal animal)
{
    public int Id => animal.Id;
    public string Name => animal.Name;
    public string? Description => animal.Description;
    public Species Species => animal.Species;
    public AnimalImageResult? MainImage { get; init; }
    public int ImageCount => animal.Images.Count;
}