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

    public static AuthenticationSession Start(
        Guid principalExternalId,
        Guid tenantExternalId,
        string authMethod,
        string assuranceLevel,
        decimal riskScore,
        Guid? deviceIdentityExternalId,
        string? ipAddress,
        string? geoLocation,
        string? federationProvider,
        bool mfaSatisfied,
        string createdBy)
    {
        var entity = new AuthenticationSession
        {
            AuthenticationSessionExternalId = Guid.NewGuid(),
            PrincipalExternalId = Guard.AgainstDefault(principalExternalId, nameof(principalExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            AuthMethod = Guard.AgainstMaxLength(authMethod, 100, nameof(authMethod)),
            AssuranceLevel = Guard.AgainstMaxLength(assuranceLevel, 50, nameof(assuranceLevel)),
            RiskScore = Guard.AgainstOutOfRange(riskScore, 0m, 1m, nameof(riskScore)),
            DeviceIdentityExternalId = deviceIdentityExternalId,
            IpAddress = string.IsNullOrWhiteSpace(ipAddress) ? null : Guard.AgainstMaxLength(ipAddress, 100, nameof(ipAddress)),
            GeoLocation = string.IsNullOrWhiteSpace(geoLocation) ? null : Guard.AgainstMaxLength(geoLocation, 200, nameof(geoLocation)),
            FederationProvider = string.IsNullOrWhiteSpace(federationProvider) ? null : Guard.AgainstMaxLength(federationProvider, 100, nameof(federationProvider)),
            MfaSatisfied = mfaSatisfied,
            StartedAt = DateTimeOffset.UtcNow,
            Status = AuthSessionStatus.Started
        };

        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void MarkMfaSatisfied(string updatedBy)
    {
        EnsureStarted();
        MfaSatisfied = true;
        Touch(updatedBy);
    }

    public void Complete(string updatedBy)
    {
        EnsureStarted();
        Status = AuthSessionStatus.Completed;
        CompletedAt = DateTimeOffset.UtcNow;
        Touch(updatedBy);
    }

    public void Fail(string updatedBy)
    {
        EnsureStarted();
        Status = AuthSessionStatus.Failed;
        CompletedAt = DateTimeOffset.UtcNow;
        Touch(updatedBy);
    }

    public void Expire(string updatedBy)
    {
        EnsureStarted();
        Status = AuthSessionStatus.Expired;
        CompletedAt = DateTimeOffset.UtcNow;
        Touch(updatedBy);
    }

    private void EnsureStarted()
    {
        if (Status != AuthSessionStatus.Started)
            throw new DomainException("Only started authentication sessions can transition.");
    }
}