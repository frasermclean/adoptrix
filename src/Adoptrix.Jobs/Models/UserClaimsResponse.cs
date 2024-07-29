using System.Text.Json.Serialization;
using Adoptrix.Core;

namespace Adoptrix.Jobs.Models;

public class UserClaimsResponse
{
    public required DataModel Data { get; init; }

    public static UserClaimsResponse Create(User user, Guid correlationId)
    {
        return new UserClaimsResponse
        {
            Data = new DataModel
            {
                Actions =
                [
                    new ActionsModel
                    {
                        Claims = new ClaimsModel
                        {
                            Role = user.Role,
                            CorrelationId = correlationId
                        }
                    }
                ]
            }
        };
    }

    public class DataModel
    {
        [JsonPropertyName("@odata.type")] public string OdataType => "microsoft.graph.onTokenIssuanceStartResponseData";

        public required ActionsModel[] Actions { get; init; }
    }

    public class ActionsModel
    {
        [JsonPropertyName("@odata.type")]
        public string OdataType => "microsoft.graph.tokenIssuanceStart.provideClaimsForToken";

        public required ClaimsModel Claims { get; init; }
    }

    public class ClaimsModel
    {
        public required UserRole Role { get; init; }
        public required Guid CorrelationId { get; init; }
    }
}
