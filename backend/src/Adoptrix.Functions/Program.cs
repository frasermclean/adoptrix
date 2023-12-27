using Adoptrix.Infrastructure.Services;
using Microsoft.Extensions.Hosting;

namespace Adoptrix.Functions;

public static class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices((context, services)=>
            {
                services.AddInfrastructureServices(context.Configuration, context.HostingEnvironment);
            })
            .Build();

        host.Run();
    }
}