using Adoptrix.Jobs.Models;
using Adoptrix.Persistence.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using User = Adoptrix.Core.User;

namespace Adoptrix.Jobs.Functions;

public class ProvideUserClaims(ILogger<ProvideUserClaims> logger, IUsersRepository usersRepository)
{
    [Function(nameof(ProvideUserClaims))]
    public async Task<HttpResponseData> ExecuteAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user-claims")]
        HttpRequestData httpRequestData,
        [FromBody] UserClaimsRequest claimsRequest,
        CancellationToken cancellationToken)
    {
        // get or create user from the repository
        var userId = claimsRequest.Data.AuthenticationContext.User.Id;
        var user = await usersRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            logger.LogInformation("Creating new user with ID {UserId}", userId);
            user = new User { Id = userId };
            await usersRepository.CreateAsync(user, cancellationToken);
        }

        // create response object and write it to the http response
        var claimsResponse = UserClaimsResponse.Create(user);
        var httpResponseData = httpRequestData.CreateResponse();
        await httpResponseData.WriteAsJsonAsync(claimsResponse, cancellationToken);

        return httpResponseData;
    }
}
