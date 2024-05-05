using Adoptrix.AI.DependencyInjection;
using Adoptrix.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.AI.Tests.Fixtures;

public class AiFixture
{
    private readonly IServiceProvider serviceProvider;
    public AiFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["OpenAi:Endpoint"] = "https://openat-test-ai.openai.azure.com/",
                ["OpenAi:DeploymentName"] = "gpt-35-turbo"
            })
            .Build();

        serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddAiServices()
            .BuildServiceProvider();
    }

    public IAnimalAssistant AnimalAssistant => serviceProvider.GetRequiredService<IAnimalAssistant>();
}
