using System.Globalization;
using System.Reflection;
using FastEndpoints;

namespace Adoptrix.Endpoints;

public class AboutEndpoint : EndpointWithoutRequest<AboutResponse>
{
    private static readonly AboutResponse AboutResponse;

    static AboutEndpoint()
    {
        var buildData = GetBuildData();

        AboutResponse = new AboutResponse(
            Version: buildData.Version,
            Environment: Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
            BuildDate: buildData.BuildDate);
    }

    public override void Configure()
    {
        Get("about");
        AllowAnonymous();
    }

    public override Task<AboutResponse> ExecuteAsync(CancellationToken _)
    {
        return Task.FromResult(AboutResponse);
    }

    private static (string Version, DateTime BuildDate) GetBuildData()
    {
        // get version string from custom attribute
        var versionString = Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            !.InformationalVersion;

        // split the version string to get the date part
        var versionParts = versionString.Split("+build");

        var version = versionParts[0];
        var buildDate = DateTime.ParseExact(versionParts[1], "yyyyMMddHHmmss", CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal);

        return (version, buildDate.ToUniversalTime());
    }
}

public record AboutResponse(string Version, string Environment, DateTime BuildDate);
