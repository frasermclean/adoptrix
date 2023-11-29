using Adoptrix.Api.Startup;

namespace Adoptrix.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var app = WebApplication.CreateBuilder(args)
            .RegisterServices()
            .Build()
            .ConfigureMiddleware();

        app.Run();
    }
}