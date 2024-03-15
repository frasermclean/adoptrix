using Microsoft.AspNetCore.Mvc;

namespace Adoptrix.Api.Contracts.Requests;

public class AddAnimalImagesRequest
{
    [FromRoute] public Guid AnimalId { get; init; }
    [FromForm] public IFormFileCollection FormFileCollection { get; init; } = null!;
}
