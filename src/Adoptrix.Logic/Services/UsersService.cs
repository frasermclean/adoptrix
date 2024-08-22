using Adoptrix.Contracts.Responses;
using FluentResults;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;

namespace Adoptrix.Logic.Services;

public interface IUsersService
{
    Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<Result<UserResponse>> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);
}

public class UsersService(GraphServiceClient serviceClient) : IUsersService
{
    private static readonly string[] QueryParameters = ["id", "givenName", "surname", "displayName", "mail"];

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var response = await serviceClient.Users.GetAsync(configuration =>
        {
            configuration.QueryParameters.Select = QueryParameters;
            configuration.QueryParameters.Orderby = ["displayName"];
        }, cancellationToken);

        return response?.Value?.Select(MapToResponse) ?? [];
    }

    public async Task<Result<UserResponse>> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await serviceClient.Users[userId.ToString()]
                .GetAsync(request => request.QueryParameters.Select = QueryParameters, cancellationToken);
            return MapToResponse(user!);
        }
        catch (ODataError)
        {
            return Result.Fail("Error fetching user from Graph API");
        }
    }

    private static UserResponse MapToResponse(User user) => new()
    {
        Id = Guid.Parse(user.Id!),
        FirstName = user.GivenName,
        LastName = user.Surname,
        DisplayName = user.DisplayName,
        EmailAddress = user.Mail,
        Role = null
    };
}
