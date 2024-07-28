using Adoptrix.Persistence.Services;
using FluentResults;
using Microsoft.Graph;
using Microsoft.Graph.Models.ODataErrors;
using GraphUser = Microsoft.Graph.Models.User;
using User = Adoptrix.Core.User;

namespace Adoptrix.Api.Services;

public interface IUsersService
{
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<Result<User>> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);
}

public class UsersService(IUsersRepository usersRepository, GraphServiceClient serviceClient) : IUsersService
{
    private static readonly string[] QueryParameters = ["id", "givenName", "surname", "displayName", "mail"];

    public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var usersTask = usersRepository.GetAllAsync(cancellationToken);
        var graphUsersTask = serviceClient.Users.GetAsync(configuration =>
        {
            configuration.QueryParameters.Select = QueryParameters;
            configuration.QueryParameters.Orderby = ["displayName"];
        }, cancellationToken);

        await Task.WhenAll(usersTask, graphUsersTask);
        var (users, response) = (usersTask.Result, graphUsersTask.Result);

        if (response?.Value is null)
        {
            return [];
        }

        foreach (var user in users)
        {
            var graphUser = response.Value.FirstOrDefault(u => u.Id == user.Id.ToString());
            if (graphUser is null)
            {
                continue;
            }

            MapGraphUserProperties(user, graphUser);
        }

        return users;
    }

    public async Task<Result<User>> GetUserAsync(Guid userId, CancellationToken cancellationToken)
    {

        var userTask = usersRepository.GetByIdAsync(userId, cancellationToken);
        var graphUserTask = serviceClient.Users[userId.ToString()]
            .GetAsync(request => request.QueryParameters.Select = QueryParameters, cancellationToken);
        try
        {
            await Task.WhenAll(userTask, graphUserTask);
        }
        catch (ODataError)
        {
            return Result.Fail("Error fetching user from Graph API");
        }

        var (user, graphUser) = (userTask.Result, graphUserTask.Result);

        if (user is null || graphUser is null)
        {
            return Result.Fail("User not found");
        }

        MapGraphUserProperties(user, graphUser);
        return user;
    }

    private static void MapGraphUserProperties(User user, GraphUser graphUser)
    {
        user.FirstName = graphUser.GivenName;
        user.LastName = graphUser.Surname;
        user.DisplayName = graphUser.DisplayName;
        user.EmailAddress = graphUser.Mail;
    }
}
