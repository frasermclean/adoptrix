using Adoptrix.AI.Options;
using Adoptrix.Application.Features.Animals.Queries;
using Adoptrix.Application.Features.Animals.Responses;
using Adoptrix.Application.Services;
using Azure.AI.OpenAI;
using Humanizer;
using Microsoft.Extensions.Options;

namespace Adoptrix.AI.Services;

public class AnimalAssistant(OpenAIClient client, IOptions<OpenAiOptions> options) : IAnimalAssistant
{
    private readonly string deploymentName = options.Value.DeploymentName;

    public async Task<GenerateAnimalDescriptionResponse> GenerateDescriptionAsync(GenerateAnimalDescriptionQuery query, CancellationToken cancellationToken)
    {
        var (animalName, breedName, speciesName, sex, dateOfBirth) = query;

        var ageSpan = DateTime.UtcNow - dateOfBirth.ToDateTime(TimeOnly.MinValue);
        var age = ageSpan.Humanize();

        var options = new ChatCompletionsOptions
        {
            DeploymentName = deploymentName,
            Messages =
            {
                new ChatRequestSystemMessage(
                    "You are a system which generates descriptions of animals that are available for adoption."),
                new ChatRequestUserMessage(
                    $"Generate a description for a {sex.ToString().ToLowerInvariant()}, {breedName}, {speciesName} named {animalName} who is currently {age}")
            }
        };

        var response = await client.GetChatCompletionsAsync(options, cancellationToken);

        return new GenerateAnimalDescriptionResponse(response.Value.Choices[0].Message.Content);
    }
}
