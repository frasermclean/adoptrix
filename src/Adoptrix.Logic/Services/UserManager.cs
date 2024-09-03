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
    private readonly Guid apiObjectId = options.Value.ApiObjectId;
    private static readonly string[] QueryParameters = ["id", "givenName", "surname", "displayName", "mail"];

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var usersTask = serviceClient.Users.GetAsync(configuration =>
        {
            configuration.QueryParameters.Select = QueryParameters;
            configuration.QueryParameters.Orderby = ["displayName"];
        }, cancellationToken);

        var appRoleAssignmentsTask = serviceClient.ServicePrincipals[apiObjectId.ToString()]
            .AppRoleAssignedTo
            .GetAsync(cancellationToken: cancellationToken);

        await Task.WhenAll(usersTask, appRoleAssignmentsTask);

        var users = usersTask.Result?.Value ?? [];
        var appRoleAssignments = appRoleAssignmentsTask.Result?.Value ?? [];

        foreach (var user in users)
        {
            var userId = Guid.Parse(user.Id!);
            var roleAssignment = appRoleAssignments.FirstOrDefault(assignment => assignment.PrincipalId == userId);
            var roleName = GetRoleName(roleAssignment);


            user.AdditionalData["Role"] = roleName;
        }

        return users.Select(MapToResponse);
    }

    public async Task<Result<UserResponse>> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var userTask = serviceClient.Users[userId.ToString()]
                .GetAsync(request => request.QueryParameters.Select = QueryParameters, cancellationToken);

            var appRoleAssignmentsTask = serviceClient.Users[userId.ToString()].AppRoleAssignments
                .GetAsync(cancellationToken: cancellationToken);

            await Task.WhenAll(userTask, appRoleAssignmentsTask);

            var user = userTask.Result;

            var apiRoleAssignment =
                appRoleAssignmentsTask.Result?.Value?.FirstOrDefault(assignment =>
                    assignment.ResourceId == apiObjectId);

            var roleName = GetRoleName(apiRoleAssignment);
            user!.AdditionalData["Role"] = roleName;

            return MapToResponse(user);
        }
        catch (ODataError)
        {
            return Result.Fail("Error fetching user from Graph API");
        }
    }

    private static string GetRoleName(AppRoleAssignment? roleAssignment)
    {
        var administratorRoleId = UserRoles.GetRoleId(UserRoles.Administrator);

        return roleAssignment?.AppRoleId == administratorRoleId
            ? UserRoles.Administrator
            : UserRoles.User;
    }

    private static UserResponse MapToResponse(User user) => new()
    {
        Id = Guid.Parse(user.Id!),
        FirstName = user.GivenName,
        LastName = user.Surname,
        DisplayName = user.DisplayName,
        EmailAddress = user.Mail,
        Role = user.AdditionalData.TryGetValue("Role", out var value) ? value.ToString() : UserRoles.User
    };
}
