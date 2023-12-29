using Adoptrix.Infrastructure.Storage.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Adoptrix.Functions;

public static class Program
{
    public static async Task Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices((context, services) =>
            {
                services.AddInfrastructureStorage(context.Configuration,
                    context.HostingEnvironment.IsDevelopment());
            })
            .Build();

        await host.RunAsync();
    }
}