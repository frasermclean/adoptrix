using System.Text.Json.Serialization;
using Adoptrix.Core;
using Adoptrix.Persistence.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Jobs.Functions;

public class GetUserClaims(ILogger<GetUserClaims> logger, IUsersRepository usersRepository)
{
    [Function(nameof(GetUserClaims))]
    public async Task<HttpResponseData> ExecuteAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{userId:guid}/claims")]
        HttpRequestData request,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByIdAsync(userId, cancellationToken);
        var response = request.CreateResponse();

        if (user is null)
        {
            logger.LogInformation("Creating new user with ID {UserId}", userId);
            user = new User { Id = userId };
            await usersRepository.CreateAsync(user, cancellationToken);
        }

        await response.WriteAsJsonAsync(new UserClaimsResponse
        {
            UserId = user.Id,
            Role = user.Role.ToString()
        }, cancellationToken);

        return response;
    }

    private class UserClaimsResponse
    {
        [JsonPropertyName("userId")] public Guid UserId { get; init; }
        [JsonPropertyName("role")] public required string Role { get; init; }
    }
}
