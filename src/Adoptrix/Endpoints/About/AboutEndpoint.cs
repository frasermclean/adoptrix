using System.Globalization;
using System.Reflection;
using Adoptrix.Core.Abstractions;
using Adoptrix.Persistence;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Adoptrix.Endpoints.About;

[HttpGet("about"), AllowAnonymous]
public class AboutEndpoint(
    [FromKeyedServices(BlobContainerNames.AnimalImages)] IBlobContainerManager blobContainerManager)
    : EndpointWithoutRequest<AboutResponse>
{
    private AboutResponse? response;

    public override Task<AboutResponse> ExecuteAsync(CancellationToken _)
    {
        if (response is not null) return Task.FromResult(response);

        var buildData = GetBuildData();
        response = new AboutResponse
        {
            Version = buildData.Version,
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
            BuildDate = buildData.BuildDate,
            AnimalImagesBaseUrl = blobContainerManager.ContainerUri
        };

        return Task.FromResult(response);
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
