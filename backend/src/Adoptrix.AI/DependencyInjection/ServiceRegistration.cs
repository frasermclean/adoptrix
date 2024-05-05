using Adoptrix.AI.Options;
using Adoptrix.AI.Services;
using Adoptrix.Application.Services;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Adoptrix.AI.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddAiServices(this IServiceCollection services)
    {
        services.AddOptions<OpenAiOptions>()
            .BindConfiguration(OpenAiOptions.SectionName)
            .ValidateDataAnnotations();

        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<OpenAiOptions>>();
            var endpointUri = new Uri(options.Value.Endpoint);
            return new OpenAIClient(endpointUri, new DefaultAzureCredential());
        });

        services.AddScoped<IAnimalAssistant, AnimalAssistant>();

        return services;
    }
}
