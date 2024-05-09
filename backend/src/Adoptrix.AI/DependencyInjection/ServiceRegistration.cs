using Adoptrix.AI.Options;
using Adoptrix.AI.Services;
using Adoptrix.Application.Services;
using Azure.Identity;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.AI.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddAiServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<OpenAiOptions>()
            .BindConfiguration(OpenAiOptions.SectionName)
            .ValidateDataAnnotations();

        services.AddAzureClients(builder =>
        {
            const string key = $"{OpenAiOptions.SectionName}:{nameof(OpenAiOptions.Endpoint)}";
            var endpoint = configuration.GetValue<string>(key);
            builder.AddOpenAIClient(new Uri(endpoint!));
            builder.UseCredential(new DefaultAzureCredential());
        });

        services.AddScoped<IAnimalAssistant, AnimalAssistant>();

        return services;
    }
}
