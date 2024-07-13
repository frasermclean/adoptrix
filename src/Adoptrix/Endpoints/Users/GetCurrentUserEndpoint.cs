﻿using Adoptrix.Extensions;
using FastEndpoints;

namespace Adoptrix.Endpoints.Users;

public class GetCurrentUserEndpoint : EndpointWithoutRequest<GetCurrentUserResponse>
{
    public override void Configure()
    {
        Get("users/me");
    }

    public override Task<GetCurrentUserResponse> ExecuteAsync(CancellationToken ct)
    {
        var response = new GetCurrentUserResponse
        {
            Name = User.GetUserName(),
            UserId = User.GetUserId()
        };

        return Task.FromResult(response);
    }
}