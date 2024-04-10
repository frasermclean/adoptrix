using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adoptrix.Application.Contracts.Requests.Animals;

public class AddAnimalImagesRequest
{
    [FromRoute] public Guid AnimalId { get; init; }
    [FromForm] public IFormFileCollection FormFileCollection { get; init; } = null!;
}
