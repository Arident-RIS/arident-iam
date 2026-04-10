using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;
using AridentIam.Domain.Events;

namespace AridentIam.Domain.Entities.Sessions;

public sealed class Session : AggregateRoot
{
    private Session() { }
    public Guid SessionExternalId { get; private set; }
    public Guid PrincipalExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public Guid AuthenticationSessionExternalId { get; private set; }
    public SessionState SessionState { get; private set; }
    public string AssuranceLevel { get; private set; } = null!;
    public DateTimeOffset StartedAt { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public DateTimeOffset? RevokedAt { get; private set; }
    public DateTimeOffset? LastActivityAt { get; private set; }

    public static Session Create(Guid principalExternalId, Guid tenantExternalId, Guid authenticationSessionExternalId, string assuranceLevel, DateTimeOffset expiresAt, string createdBy)
    {
        if (expiresAt <= DateTimeOffset.UtcNow)
            throw new DomainException("Session expiry must be in the future.");

        var entity = new Session
        {
            SessionExternalId = Guid.NewGuid(),
            PrincipalExternalId = Guard.AgainstDefault(principalExternalId, nameof(principalExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            AuthenticationSessionExternalId = Guard.AgainstDefault(authenticationSessionExternalId, nameof(authenticationSessionExternalId)),
            SessionState = SessionState.Active,
            AssuranceLevel = Guard.AgainstNullOrWhiteSpace(assuranceLevel, nameof(assuranceLevel)),
            StartedAt = DateTimeOffset.UtcNow,
            ExpiresAt = expiresAt
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void RegisterActivity(DateTimeOffset activityAt, string updatedBy)
    {
        if (SessionState != SessionState.Active)
            throw new DomainException("Cannot register activity on an inactive session.");
        LastActivityAt = activityAt;
        Touch(updatedBy);
    }

    public void Revoke(string updatedBy)
    {
        if (SessionState == SessionState.Revoked)
            throw new DomainException("Session is already revoked.");
        SessionState = SessionState.Revoked;
        RevokedAt = DateTimeOffset.UtcNow;
        Touch(updatedBy);
        RaiseDomainEvent(new SessionRevokedDomainEvent(SessionExternalId));
    }
}
