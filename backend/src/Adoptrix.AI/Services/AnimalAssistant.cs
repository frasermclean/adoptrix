using System.Text;
using Adoptrix.AI.Options;
using Adoptrix.Application.Features.Generators.Queries;
using Adoptrix.Application.Features.Generators.Responses;
using Adoptrix.Application.Services;
using Azure.AI.OpenAI;
using Humanizer;
using Microsoft.Extensions.Options;

namespace Adoptrix.AI.Services;

public class AnimalAssistant(OpenAIClient client, IOptions<OpenAiOptions> options) : IAnimalAssistant
{
    private readonly string deploymentName = options.Value.DeploymentName;

    public async Task<AnimalNameResponse> GenerateNameAsync(GenerateAnimalNameQuery query,
        CancellationToken cancellationToken = default)
    {
        const string systemMessageContent =
            "You are a system which generates names for animals that are available for adoption.";

        var userMessageContent = new StringBuilder("Generate a name for a ")
            .Append(query.Sex is not null ? $"{query.Sex} " : string.Empty)
            .Append(query.BreedName is not null ? $"{query.BreedName} " : string.Empty)
            .Append(query.SpeciesName ?? "animal")
            .ToString();

        var options = CreateOptions(systemMessageContent, userMessageContent);
        var response = await client.GetChatCompletionsAsync(options, cancellationToken);

        return new AnimalNameResponse(response.Value.Choices[0].Message.Content);
    }

    public async Task<AnimalDescriptionResponse> GenerateDescriptionAsync(GenerateAnimalDescriptionQuery query,
        CancellationToken cancellationToken)
    {
        const string systemMessageContent =
            "You are a system which generates descriptions of animals that are available for adoption.";

        var (animalName, breedName, speciesName, sex, dateOfBirth) = query;
        var age = (DateTime.UtcNow - dateOfBirth.ToDateTime(TimeOnly.MinValue)).ToAge();

        var userMessageContent =
            $"Generate a description for a {sex.ToString().ToLowerInvariant()}, {breedName}, {speciesName} named {animalName} who is currently {age}";

        var options = CreateOptions(systemMessageContent, userMessageContent);
        var response = await client.GetChatCompletionsAsync(options, cancellationToken);

        return new AnimalDescriptionResponse(response.Value.Choices[0].Message.Content);
    }

    private ChatCompletionsOptions CreateOptions(string systemMessageContent, string userMessageContent) => new()
    {
        DeploymentName = deploymentName,
        Messages =
        {
            new ChatRequestSystemMessage(systemMessageContent),
            new ChatRequestUserMessage(userMessageContent)
        }
    };
}
