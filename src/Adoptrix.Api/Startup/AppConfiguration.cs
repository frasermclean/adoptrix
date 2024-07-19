using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace Adoptrix.Api.Startup;

public static class AppConfiguration
{
    public static WebApplicationBuilder AddAzureAppConfiguration(this WebApplicationBuilder builder)
    {
        var endpoint = builder.Configuration["APP_CONFIG_ENDPOINT"];
        if (string.IsNullOrEmpty(endpoint))
        {
            Console.WriteLine("Azure App Configuration endpoint not found. Using local configuration only.");
            return builder;
        }

        builder.Configuration.AddAzureAppConfiguration(options =>
        {
            options.Connect(new Uri(endpoint), new DefaultAzureCredential())
                .Select(KeyFilter.Any)
                .Select(KeyFilter.Any, builder.Environment.EnvironmentName);
        });

        return builder;
    }
}
