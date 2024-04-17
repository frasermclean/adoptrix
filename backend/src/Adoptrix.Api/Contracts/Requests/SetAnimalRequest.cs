using Adoptrix.Domain.Models;

namespace Adoptrix.Api.Contracts.Requests;

public record SetAnimalRequest(string Name, string? Description, Guid BreedId, Sex Sex, DateOnly DateOfBirth);
