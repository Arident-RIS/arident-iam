using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Sessions;

public sealed class Token : AuditableEntity
{
    private Token() { }
    public Guid TokenExternalId { get; private set; }
    public Guid SessionExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public TokenType TokenType { get; private set; }
    public string TokenIdentifier { get; private set; } = null!;
    public string Audience { get; private set; } = null!;
    public DateTimeOffset IssuedAt { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public DateTimeOffset? RevokedAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public string ClaimsHash { get; private set; } = null!;

    public static Token Create(Guid sessionExternalId, Guid tenantExternalId, TokenType tokenType, string tokenIdentifier, string audience, DateTimeOffset expiresAt, string claimsHash, string createdBy)
    {
        if (expiresAt <= DateTimeOffset.UtcNow)
            throw new DomainException("Token expiry must be in the future.");

        var entity = new Token
        {
            TokenExternalId = Guid.NewGuid(),
            SessionExternalId = Guard.AgainstDefault(sessionExternalId, nameof(sessionExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            TokenType = tokenType,
            TokenIdentifier = Guard.AgainstNullOrWhiteSpace(tokenIdentifier, nameof(tokenIdentifier)),
            Audience = Guard.AgainstNullOrWhiteSpace(audience, nameof(audience)),
            IssuedAt = DateTimeOffset.UtcNow,
            ExpiresAt = expiresAt,
            ClaimsHash = Guard.AgainstNullOrWhiteSpace(claimsHash, nameof(claimsHash)),
            IsRevoked = false
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Revoke(string updatedBy)
    {
        if (IsRevoked)
            throw new DomainException("Token is already revoked.");
        IsRevoked = true;
        RevokedAt = DateTimeOffset.UtcNow;
        Touch(updatedBy);
    }

    public bool IsExpired() => DateTimeOffset.UtcNow >= ExpiresAt;
    public bool IsValid() => !IsRevoked && !IsExpired();
}
