using Adoptrix.Domain.Models;

namespace Adoptrix.Application.Services;

/// <summary>
/// Assistant service which uses artificial intelligence to generate data for an animal.
/// </summary>
public interface IAnimalAssistant
{
    Task<string> GenerateDescriptionAsync(string name, Breed breed, Sex sex, DateOnly dateOfBirth);
}
