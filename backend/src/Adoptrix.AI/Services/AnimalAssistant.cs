using Adoptrix.AI.Options;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;

namespace Adoptrix.AI.Services;

public class AnimalAssistant(OpenAIClient client, IOptions<OpenAiOptions> options) : IAnimalAssistant
{
    private readonly string deploymentName = options.Value.DeploymentName;

    public Task<string> GenerateDescriptionAsync(string name, Breed breed, Sex sex, DateOnly dateOfBirth)
    {
        var description = string.Empty;
        return Task.FromResult(description);
    }
}
