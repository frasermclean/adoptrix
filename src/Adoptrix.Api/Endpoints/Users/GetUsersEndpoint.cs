using Adoptrix.Api.Extensions;
using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Web;

namespace Adoptrix.Api.Endpoints.Users;

[HttpGet("users")]
public class GetUsersEndpoint(GraphServiceClient graphServiceClient, IUsersRepository usersRepository)
    : EndpointWithoutRequest<IEnumerable<UserResponse>>
{
    public override async Task<IEnumerable<UserResponse>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var graphUsersTask = GetGraphUsersAsync(cancellationToken);
        var repositoryUsersTask = usersRepository.GetAllAsync(cancellationToken);

        await Task.WhenAll(graphUsersTask, repositoryUsersTask);

        if (graphUsersTask.Result?.Value is null)
        {
            return [];
        }

        return graphUsersTask.Result.Value.Select(user =>
        {
            var userId = Guid.Parse(user.Id!);
            return new UserResponse
            {
                Id = userId,
                FirstName = user.GivenName,
                LastName = user.Surname,
                DisplayName = user.DisplayName,
                EmailAddress = user.Mail,
                Role = repositoryUsersTask.Result.FirstOrDefault(u => u.Id == userId)?.Role.ToString(),
                IsCurrentUser = User.GetUserId() == userId
            };
        }).Where(response => response.Role is not null);
    }

    private async Task<UserCollectionResponse?> GetGraphUsersAsync(CancellationToken cancellationToken)
    {
        var response = await graphServiceClient.Users.GetAsync(configuration =>
        {
            configuration.QueryParameters.Select = ["id", "givenName", "surname", "displayName", "mail"];
            configuration.QueryParameters.Orderby = ["displayName"];
        }, cancellationToken);

        return response;
    }
}
