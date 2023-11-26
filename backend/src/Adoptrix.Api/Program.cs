using Adoptrix.Api.Startup;

namespace Adoptrix.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplication.CreateBuilder(args)
            .RegisterServices()
            .Build()
            .ConfigureMiddleware()
            .Run();
    }
}