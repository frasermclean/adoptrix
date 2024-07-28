using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Jobs.Functions;

public class GetUserClaims(ILogger<GetUserClaims> logger)
{
    [Function(nameof(GetUserClaims))]
    public async Task<HttpResponseData> ExecuteAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{userId:guid}/claims")]
        HttpRequestData request,
        Guid userId)
    {
        logger.LogInformation("C# HTTP trigger function processed a request");

        var response = request.CreateResponse();
        await response.WriteAsJsonAsync(new UserClaimsResponse
        {
            UserId = userId,
            Role = "user"
        });

        return response;
    }

    private class UserClaimsResponse
    {
        public Guid UserId { get; init; }
        public string Role { get; init; }
    }
}
