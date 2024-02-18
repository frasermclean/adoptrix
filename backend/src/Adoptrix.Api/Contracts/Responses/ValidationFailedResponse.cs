namespace Adoptrix.Api.Contracts.Responses;

public sealed record ValidationFailedResponse(string Message = "Validation failure") : MessageResponse(Message)
{
    public IEnumerable<string> Errors { get; init; } = Enumerable.Empty<string>();
}
