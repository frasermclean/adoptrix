using Adoptrix.Web.Startup;

namespace Adoptrix.Web;

public class Program
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