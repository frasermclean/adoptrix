using Adoptrix.ServiceDefaults;
using Adoptrix.Web.Startup;

namespace Adoptrix.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var app = WebApplication.CreateBuilder(args)
            .AddServiceDefaults()
            .RegisterServices()
            .Build()
            .ConfigureMiddleware();

        app.Run();
    }
}
