﻿using Adoptrix.Api.Endpoints.Users;
using Adoptrix.Api.Errors;
using Adoptrix.Api.Options;
using Adoptrix.Api.Security;
using Adoptrix.Core;
using FluentResults;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;

namespace Adoptrix.Api.Services;

public interface IUserManager
{
    Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<Result<UserResponse>> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<Result<UserResponse>> AddUserRoleAssignmentAsync(Guid userId, UserRole role,
        CancellationToken cancellationToken = default);

    Task<Result<UserResponse>> RemoveUserRoleAssignmentAsync(Guid userId, UserRole role,
        CancellationToken cancellationToken = default);
}

public class UserManager(
    GraphServiceClient serviceClient,
    IOptions<UserManagerOptions> options,
    ILogger<UserManager> logger) : IUserManager
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

        return users.Select(user =>
        {
            var userId = Guid.Parse(user.Id!);
            var userRoles = appRoleAssignments.Where(assignment => assignment.PrincipalId == userId)
                .Select(GetUserRole);

            return MapToResponse(user, userRoles);
        });
    }

    public async Task<Result<UserResponse>> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userIdString = userId.ToString();

        try
        {
            var userTask = serviceClient.Users[userIdString]
                .GetAsync(request => request.QueryParameters.Select = QueryParameters, cancellationToken);

            var appRoleAssignmentsTask = serviceClient.Users[userIdString].AppRoleAssignments
                .GetAsync(cancellationToken: cancellationToken);

            await Task.WhenAll(userTask, appRoleAssignmentsTask);

            var user = userTask.Result!;
            var userRoles = appRoleAssignmentsTask.Result?.Value?
                .Where(assignment => assignment.ResourceId == apiObjectId)
                .Select(GetUserRole);

            return MapToResponse(user, userRoles);
        }
        catch (ODataError error) when (error.ResponseStatusCode == 404)
        {
            return new UserNotFoundError(userId).CausedBy(error);
        }
    }

    public async Task<Result<UserResponse>> AddUserRoleAssignmentAsync(Guid userId, UserRole role,
        CancellationToken cancellationToken = default)
    {
        var appRoleId = AppRoleIdMapping.GetAppRoleId(role);

        try
        {
            await serviceClient.ServicePrincipals[apiObjectId.ToString()].AppRoleAssignedTo
                .PostAsync(new AppRoleAssignment
                {
                    PrincipalId = userId,
                    AppRoleId = appRoleId,
                    ResourceId = apiObjectId
                }, cancellationToken: cancellationToken);
        }
        catch (ODataError error) when (error.ResponseStatusCode == 400)
        {
            logger.LogError(error, "Could not assign role {Role} to user with ID {UserId}", role, userId);
            return new UserRoleAlreadyAssignedError(role).CausedBy(error);
        }
        catch (ODataError error) when (error.ResponseStatusCode == 404)
        {
            return new UserNotFoundError(userId).CausedBy(error);
        }

        logger.LogInformation("Assigned role {Role} to user with ID {UserId}", role, userId);
        return await GetUserAsync(userId, cancellationToken);
    }

    public async Task<Result<UserResponse>> RemoveUserRoleAssignmentAsync(Guid userId, UserRole role,
        CancellationToken cancellationToken = default)
    {
        var appRoleId = AppRoleIdMapping.GetAppRoleId(role);
        var appRoleAssignments = await serviceClient.ServicePrincipals[apiObjectId.ToString()].AppRoleAssignedTo
            .GetAsync(cancellationToken: cancellationToken);

        var appRoleAssignmentId = appRoleAssignments?.Value?
            .Where(assignment => assignment.PrincipalId == userId && assignment.AppRoleId == appRoleId)
            .Select(assignment => assignment.Id)
            .FirstOrDefault();

        if (appRoleAssignmentId is null)
        {
            logger.LogError("User with ID {UserId} does not have role {Role}", userId, role);
            return new UserRoleNotAssignedError(role);
        }

        await serviceClient.ServicePrincipals[apiObjectId.ToString()].AppRoleAssignedTo[appRoleAssignmentId]
            .DeleteAsync(cancellationToken: cancellationToken);

        logger.LogInformation("Removed role {Role} from user with ID {UserId}", role, userId);
        return await GetUserAsync(userId, cancellationToken);
    }

    private static UserRole GetUserRole(AppRoleAssignment? roleAssignment) =>
        roleAssignment?.AppRoleId == AppRoleIdMapping.GetAppRoleId(UserRole.Administrator)
            ? UserRole.Administrator
            : default;

    private static UserResponse MapToResponse(User user, IEnumerable<UserRole>? roles = null) => new()
    {
        Id = Guid.Parse(user.Id!),
        FirstName = user.GivenName,
        LastName = user.Surname,
        DisplayName = user.DisplayName,
        EmailAddress = user.Mail,
        Roles = roles ?? []
    };
}
