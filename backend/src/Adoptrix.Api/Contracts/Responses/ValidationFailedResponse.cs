namespace Adoptrix.Api.Contracts.Responses;

public class ValidationFailedResponse
{
    public required string Message { get; init; }
    public IEnumerable<string> Errors { get; init; } = Enumerable.Empty<string>();
}
