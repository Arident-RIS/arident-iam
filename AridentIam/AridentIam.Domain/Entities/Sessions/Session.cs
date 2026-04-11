using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;
using AridentIam.Domain.Events;

namespace AridentIam.Domain.Entities.Sessions;

public sealed class Session : AggregateRoot
{
    private readonly List<Token> _tokens = [];

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

    public IReadOnlyCollection<Token> Tokens => _tokens.AsReadOnly();

    public static Session Create(
        Guid principalExternalId,
        Guid tenantExternalId,
        Guid authenticationSessionExternalId,
        string assuranceLevel,
        DateTimeOffset expiresAt,
        string createdBy)
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
            AssuranceLevel = Guard.AgainstMaxLength(assuranceLevel, 50, nameof(assuranceLevel)),
            StartedAt = DateTimeOffset.UtcNow,
            ExpiresAt = expiresAt
        };

        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public Token IssueToken(
        TokenType tokenType,
        string tokenIdentifier,
        string audience,
        DateTimeOffset expiresAt,
        string claimsHash,
        string createdBy)
    {
        EnsureActive();

        if (expiresAt > ExpiresAt)
            throw new DomainException("Token expiry cannot exceed session expiry.");

        var token = Token.Create(
            sessionExternalId: SessionExternalId,
            tenantExternalId: TenantExternalId,
            tokenType: tokenType,
            tokenIdentifier: tokenIdentifier,
            audience: audience,
            expiresAt: expiresAt,
            claimsHash: claimsHash,
            createdBy: createdBy);

        _tokens.Add(token);
        Touch(createdBy);
        return token;
    }

    public void RegisterActivity(DateTimeOffset activityAt, string updatedBy)
    {
        EnsureActive();

        if (activityAt < StartedAt)
            throw new DomainException("Activity time cannot be before the session start.");

        if (activityAt > ExpiresAt)
            throw new DomainException("Activity time cannot be after the session expiry.");

        LastActivityAt = activityAt;
        Touch(updatedBy);
    }

    public void RevokeToken(Guid tokenExternalId, string updatedBy)
    {
        var token = _tokens.SingleOrDefault(x => x.TokenExternalId == tokenExternalId);
        if (token is null)
            throw new DomainException("Token does not belong to this session.");

        token.Revoke(updatedBy);
        Touch(updatedBy);
    }

    public void RevokeAllTokens(string updatedBy)
    {
        foreach (var token in _tokens)
            token.Revoke(updatedBy);

        Touch(updatedBy);
    }

    public void Expire(string updatedBy)
    {
        if (SessionState == SessionState.Revoked)
            throw new DomainException("Revoked sessions cannot transition to expired.");

        SessionState = SessionState.Expired;
        RevokeAllTokens(updatedBy);
        Touch(updatedBy);
    }

    public void Terminate(string updatedBy)
    {
        if (SessionState == SessionState.Terminated)
            return;

        SessionState = SessionState.Terminated;
        RevokeAllTokens(updatedBy);
        Touch(updatedBy);
    }

    public void Revoke(string updatedBy)
    {
        if (SessionState == SessionState.Revoked)
            throw new DomainException("Session is already revoked.");

        SessionState = SessionState.Revoked;
        RevokedAt = DateTimeOffset.UtcNow;
        RevokeAllTokens(updatedBy);
        Touch(updatedBy);
        RaiseDomainEvent(new SessionRevokedDomainEvent(SessionExternalId));
    }

    public bool IsActive()
    {
        return SessionState == SessionState.Active && DateTimeOffset.UtcNow < ExpiresAt && RevokedAt is null;
    }

    private void EnsureActive()
    {
        if (!IsActive())
            throw new DomainException("Session is not active.");
    }
}