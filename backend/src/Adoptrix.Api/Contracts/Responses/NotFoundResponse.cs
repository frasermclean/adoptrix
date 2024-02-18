namespace Adoptrix.Api.Contracts.Responses;

public class NotFoundResponse : IMessageResponse
{
    public required string Message { get; init; }

    public static NotFoundResponse Create(string message) => new()
    {
        Message = message
    };
}
