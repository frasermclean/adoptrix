using Adoptrix.Application.DependencyInjection;
using Adoptrix.Application.Features.Animals.Commands;
using Adoptrix.Database.DependencyInjection;
using Adoptrix.Storage.DependencyInjection;
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
                    options.Connect(new Uri(endpoint), new DefaultAzureCredential())
                        .Select(KeyFilter.Any)
                        .Select(KeyFilter.Any, context.HostingEnvironment.EnvironmentName);
                });
            })
            .ConfigureServices((context, services) =>
            {
                // application insights
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();

                services.AddMediatR(configuration =>
                    configuration.RegisterServicesFromAssemblyContaining<AddAnimalCommandHandler>());

                // local project services
                services.AddApplicationServices();
                services.AddDatabaseServices();
                services.AddStorageServices(context.Configuration);
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
