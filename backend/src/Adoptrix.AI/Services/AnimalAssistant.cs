using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;

namespace Adoptrix.AI.Services;

public class AnimalAssistant : IAnimalAssistant
{
    public Task<string> GenerateDescriptionAsync(string name, Breed breed, Sex sex, DateOnly dateOfBirth)
    {
        throw new NotImplementedException();
    }
}
