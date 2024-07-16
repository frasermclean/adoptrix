using Adoptrix.ServiceDefaults;

namespace Adoptrix.Initializer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.AddServiceDefaults();

        builder.Services.AddHostedService<WorkerService>();

        var host = builder.Build();
        host.Run();
    }
}
