using Adoptrix.Initializer.Services;
using Adoptrix.Persistence.Services;
using Adoptrix.ServiceDefaults;

namespace Adoptrix.Initializer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.AddServiceDefaults()
            .AddPersistence();

        builder.Services.AddHostedService<WorkerService>()
            .AddSingleton<StorageInitializer>()
            .AddScoped<DatabaseInitializer>();

        var host = builder.Build();
        host.Run();
    }
}
