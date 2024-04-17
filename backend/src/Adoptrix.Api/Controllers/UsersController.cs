using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Adoptrix.Api.Controllers;

public class UsersController : ApiController
{
    [HttpGet("me")]
    public UserResponse GetCurrentUser()
    {
        return new UserResponse
        {
            Id = User.GetUserId()
        };
    }
}
