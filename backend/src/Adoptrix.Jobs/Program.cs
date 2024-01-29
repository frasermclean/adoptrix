using Adoptrix.Application.Services;
using Adoptrix.Infrastructure.DependencyInjection;
using Microsoft.Azure.Functions.Worker;
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
            .ConfigureServices((context, services) =>
            {
                // application insights
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();

                // local project services
                services.AddApplicationServices();
                services.AddInfrastructureServices(context.Configuration);
            })
            .ConfigureLogging(builder =>
            {
                builder.Services.Configure<LoggerFilterOptions>(options =>
                {
                    // remove default rule which excludes information level logs from Application Insights
                    const string appInsightsProviderName = "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider";
                    var defaultRule = options.Rules.FirstOrDefault(rule => rule.ProviderName == appInsightsProviderName);
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