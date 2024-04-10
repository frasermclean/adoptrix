using Adoptrix.Domain.Models;

namespace Adoptrix.Api.Contracts.Data;

public record SetAnimalData(string Name, string? Description, Guid BreedId, Sex Sex, DateOnly DateOfBirth);
