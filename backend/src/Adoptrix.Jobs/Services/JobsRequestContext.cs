using Adoptrix.Core;

namespace Adoptrix.Jobs.Services;

public class JobsRequestContext : IRequestContext
{
    public bool IsAuthenticated => false;
    public Guid UserId => Guid.Empty;
}
