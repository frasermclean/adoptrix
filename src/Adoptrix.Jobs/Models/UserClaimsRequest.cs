using System.Text.Json.Serialization;

namespace Adoptrix.Jobs.Models;

public class UserClaimsRequest
{
    public string Type { get; init; } = string.Empty;
    public string Source { get; init; } = string.Empty;
    public DataModel Data { get; init; } = new();

    public class DataModel
    {
        [JsonPropertyName("@odata.type")] public string OdataType { get; init; } = string.Empty;
        public Guid TenantId { get; init; }
        public Guid AuthenticationEventListenerId { get; init; }
        public string CustomAuthenticationExtensionId { get; init; } = string.Empty;
        public AuthenticationContextModel AuthenticationContext { get; init; } = new();
    }

    public class AuthenticationContextModel
    {
        public Guid CorrelationId { get; init; }
        public ClientModel Client { get; init; } = new();
        public string Protocol { get; init; } = string.Empty;
        public ServicePrincicpalModel ClientServicePrincipal { get; init; } = new();
        public ServicePrincicpalModel ResourceServicePrincipal { get; init; } = new();
        public UserModel User { get; init; } = new();
    }

    public class ClientModel
    {
        [JsonPropertyName("ip")] public string IpAddress { get; init; } = string.Empty;
        public string Locale { get; init; } = string.Empty;
        public string Market { get; init; } = string.Empty;
    }

    public class ServicePrincicpalModel
    {
        public string Id { get; init; } = string.Empty;
        public string AppId { get; init; } = string.Empty;
        public string AppDisplayName { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
    }

    public class UserModel
    {
        public Guid Id { get; init; }
        public string CompanyName { get; init; } = string.Empty;
        public string CreatedDateTime { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string GivenName { get; init; } = string.Empty;
        public string Mail { get; init; } = string.Empty;
        public string OnPremisesSamAccountName { get; init; } = string.Empty;
        public string OnPremisesSecurityIdentifier { get; init; } = string.Empty;
        public string OnPremisesUserPrincipalName { get; init; } = string.Empty;
        public string PreferredLanguage { get; init; } = string.Empty;
        public string Surname { get; init; } = string.Empty;
        public string UserPrincipalName { get; init; } = string.Empty;
        public string UserType { get; init; } = string.Empty;
    }
}
