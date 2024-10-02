using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Logic;
using Adoptrix.Persistence.Services;
using Azure.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Jobs;

public static class Program
{
    public static async Task Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureAppConfiguration((context, builder) =>
            {
                var endpoint = Environment.GetEnvironmentVariable("APP_CONFIG_ENDPOINT");
                if (string.IsNullOrEmpty(endpoint))
                {
                    return;
                }

                builder.AddAzureAppConfiguration(options =>
                {
                    var credential = TokenCredentialFactory.Create();
                    options.Connect(new Uri(endpoint), credential)
                        .Select(KeyFilter.Any)
                        .Select(KeyFilter.Any, context.HostingEnvironment.EnvironmentName)
                        .ConfigureKeyVault(keyVaultOptions => keyVaultOptions.SetCredential(credential));
                });
            })
            .ConfigureServices((context, services) =>
            {
                // application insights
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();

                // local project services
                services.AddLogicServices(context.Configuration);
                services.AddPersistence(context.Configuration);

                // configure JSON serialization options
                services.Configure<JsonSerializerOptions>(options =>
                {
                    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.Converters.Add(new JsonStringEnumConverter());
                });
            })
            .ConfigureLogging(builder =>
            {
                builder.Services.Configure<LoggerFilterOptions>(options =>
                {
                    // remove default rule which excludes information level logs from Application Insights
                    const string appInsightsProviderName =
                        "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider";
                    var defaultRule =
                        options.Rules.FirstOrDefault(rule => rule.ProviderName == appInsightsProviderName);
                    if (defaultRule is not null)
                    {
                        options.Rules.Remove(defaultRule);
                    }
                });
            })
            .Build();

        await host.RunAsync();
    }
}
