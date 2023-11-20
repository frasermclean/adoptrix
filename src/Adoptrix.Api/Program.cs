using Adoptrix.Infrastructure;
using FastEndpoints;

namespace Adoptrix.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddFastEndpoints();
        builder.Services.AddDbContext<AdoptrixDbContext>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
            app.UseDeveloperExceptionPage();
        }

        app.UseFastEndpoints(config => { config.Endpoints.RoutePrefix = "api"; });
        app.Run();
    }
}