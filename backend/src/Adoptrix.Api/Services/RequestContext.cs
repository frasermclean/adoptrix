using Adoptrix.Core;

namespace Adoptrix.Api.Services;

public class RequestContext : IRequestContext
{
    public bool IsAuthenticated => UserId != Guid.Empty;
    public Guid UserId { get; init; }
}
