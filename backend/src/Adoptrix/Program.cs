using Adoptrix.Startup;

namespace Adoptrix;

public class Program
{
    public static void Main(string[] args)
    {
        var app = WebApplication.CreateBuilder(args)
            .AddAzureAppConfiguration()
            .RegisterServices()
            .Build()
            .ConfigureMiddleware();

        app.Run();
    }
}
