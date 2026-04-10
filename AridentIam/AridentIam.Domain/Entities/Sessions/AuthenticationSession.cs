using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Sessions;

public sealed class AuthenticationSession : AggregateRoot
{
    private AuthenticationSession() { }
    public Guid AuthenticationSessionExternalId { get; private set; }
    public Guid PrincipalExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string AuthMethod { get; private set; } = null!;
    public string? FederationProvider { get; private set; }
    public bool MfaSatisfied { get; private set; }
    public string AssuranceLevel { get; private set; } = null!;
    public decimal RiskScore { get; private set; }
    public Guid? DeviceIdentityExternalId { get; private set; }
    public string? IpAddress { get; private set; }
    public string? GeoLocation { get; private set; }
    public DateTimeOffset StartedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public AuthSessionStatus Status { get; private set; }

    public static AuthenticationSession Start(Guid principalExternalId, Guid tenantExternalId, string authMethod, string assuranceLevel, decimal riskScore, Guid? deviceIdentityExternalId, string? ipAddress, string? geoLocation, string? federationProvider, bool mfaSatisfied, string createdBy)
    {
        var entity = new AuthenticationSession
        {
            AuthenticationSessionExternalId = Guid.NewGuid(),
            PrincipalExternalId = Guard.AgainstDefault(principalExternalId, nameof(principalExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            AuthMethod = Guard.AgainstNullOrWhiteSpace(authMethod, nameof(authMethod)),
            AssuranceLevel = Guard.AgainstNullOrWhiteSpace(assuranceLevel, nameof(assuranceLevel)),
            RiskScore = Guard.AgainstOutOfRange(riskScore, 0m, 1m, nameof(riskScore)),
            DeviceIdentityExternalId = deviceIdentityExternalId,
            IpAddress = string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress.Trim(),
            GeoLocation = string.IsNullOrWhiteSpace(geoLocation) ? null : geoLocation.Trim(),
            FederationProvider = string.IsNullOrWhiteSpace(federationProvider) ? null : federationProvider.Trim(),
            MfaSatisfied = mfaSatisfied,
            StartedAt = DateTimeOffset.UtcNow,
            Status = AuthSessionStatus.Started
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Complete(string updatedBy)
    {
        if (Status != AuthSessionStatus.Started)
            throw new DomainException("Only started authentication sessions can be completed.");
        Status = AuthSessionStatus.Completed;
        CompletedAt = DateTimeOffset.UtcNow;
        Touch(updatedBy);
    }

    public void Fail(string updatedBy)
    {
        if (Status != AuthSessionStatus.Started)
            throw new DomainException("Only started authentication sessions can be marked as failed.");
        Status = AuthSessionStatus.Failed;
        CompletedAt = DateTimeOffset.UtcNow;
        Touch(updatedBy);
    }
}
