using Adoptrix.Api.Startup;
using Adoptrix.ServiceDefaults;

namespace Adoptrix.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var app = WebApplication.CreateBuilder(args)
            .AddAzureAppConfiguration()
            .AddServiceDefaults()
            .RegisterServices()
            .Build()
            .ConfigureMiddleware();

        app.Run();
    }
}
