using System.Net.Http.Json;
using Adoptrix.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Adoptrix.Client.Services;

public interface IGraphApiClient
{
    Task<User> GetCurrentUserAsync(CancellationToken cancellationToken = default);
}

public class GraphApiClient(HttpClient httpClient) : IGraphApiClient
{
    public async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        const string requestUri = "me?$select=id,givenName,surname,displayName,mail";
        var user = await httpClient.GetFromJsonAsync<User>(requestUri, cancellationToken);

        return user!;
    }
}

public class GraphApiAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public GraphApiAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager,
        IConfiguration configuration) : base(provider, navigationManager)
    {
        ConfigureHandler(
            [configuration["MicrosoftGraph:BaseUrl"]!],
            configuration.GetSection("MicrosoftGraph:Scopes").Get<List<string>>());
    }
}
