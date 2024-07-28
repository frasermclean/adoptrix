using Adoptrix.Contracts.Responses;
using FastEndpoints;
using Microsoft.Graph;

namespace Adoptrix.Api.Endpoints.Users;

[HttpGet("users")]
public class GetUsersEndpoint(GraphServiceClient graphServiceClient) : EndpointWithoutRequest<IEnumerable<UserResponse>>
{
    public override async Task<IEnumerable<UserResponse>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var response = await graphServiceClient.Users.GetAsync(configuration =>
        {
            configuration.QueryParameters.Select = ["id", "givenName", "surname", "displayName", "mail"];
            configuration.QueryParameters.Orderby = ["displayName"];
        }, cancellationToken);

        if (response?.Value is null)
        {
            return [];
        }

        return response.Value.Select(user => new UserResponse
        {
            Id = user.Id ?? string.Empty,
            FirstName = user.GivenName,
            LastName = user.Surname,
            DisplayName = user.DisplayName,
            EmailAddress = user.Mail
        });
    }
}
