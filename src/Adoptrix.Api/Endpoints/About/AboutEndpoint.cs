using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace Adoptrix.Api.Endpoints.About;

[HttpGet("about"), AllowAnonymous]
public class AboutEndpoint : EndpointWithoutRequest<AboutResponse>
{
    private readonly AboutResponse response;

    public AboutEndpoint()
    {
        var buildData = GetBuildData();
        response = new AboutResponse
        {
            Version = buildData.Version,
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
            BuildDate = buildData.BuildDate
        };
    }

    public override Task<AboutResponse> ExecuteAsync(CancellationToken _) => Task.FromResult(response);

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
