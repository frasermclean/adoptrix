namespace Adoptrix.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
            app.UseDeveloperExceptionPage();
        }

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}