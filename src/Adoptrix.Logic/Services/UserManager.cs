using Adoptrix.Contracts.Responses;
using Adoptrix.Logic.Options;
using FluentResults;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;

namespace Adoptrix.Logic.Services;

public interface IUserManager
{
    Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<Result<UserResponse>> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);
}

public class UserManager(GraphServiceClient serviceClient, IOptions<UserManagerOptions> options) : IUserManager
{
    private readonly string apiObjectId = options.Value.ApiObjectId;
    private static readonly string[] QueryParameters = ["id", "givenName", "surname", "displayName", "mail"];

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var appRoleAssignmentsTask = serviceClient.ServicePrincipals[apiObjectId]
            .AppRoleAssignedTo
            .GetAsync(cancellationToken: cancellationToken);

        var usersTask = serviceClient.Users.GetAsync(configuration =>
        {
            configuration.QueryParameters.Select = QueryParameters;
            configuration.QueryParameters.Orderby = ["displayName"];
        }, cancellationToken);

        await Task.WhenAll(usersTask, appRoleAssignmentsTask);

        var users = usersTask.Result?.Value ?? [];
        var appRoleAssignments = appRoleAssignmentsTask.Result?.Value ?? [];

        foreach (var user in users)
        {
            var appRoleAssignment =
                appRoleAssignments.FirstOrDefault(assignment => assignment.PrincipalId == Guid.Parse(user.Id!));
            user.AdditionalData["role"] = appRoleAssignment?.AppRoleId.ToString();
        }

        return users.Select(MapToResponse);
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
