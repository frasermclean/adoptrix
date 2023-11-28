using Adoptrix.Api.Startup;
using Adoptrix.Infrastructure;

namespace Adoptrix.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var app = WebApplication.CreateBuilder(args)
            .RegisterServices()
            .Build()
            .ConfigureMiddleware();

        var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AdoptrixDbContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        app.Run();
    }
}