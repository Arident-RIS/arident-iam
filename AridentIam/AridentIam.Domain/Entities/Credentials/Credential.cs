using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Credentials;

public sealed class Credential : AggregateRoot
{
    private Credential() { }

    public Guid CredentialExternalId { get; private set; }
    public Guid PrincipalExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public CredentialType CredentialType { get; private set; }
    public CredentialStatus Status { get; private set; }

    public string Verifier { get; private set; } = null!;
    public string? Salt { get; private set; }
    public int Version { get; private set; }
    public DateTimeOffset? ExpiresAt { get; private set; }
    public DateTimeOffset? RevokedAt { get; private set; }
    public DateTimeOffset? LastUsedAt { get; private set; }
    public DateTimeOffset? LockoutEndAt { get; private set; }
    public int FailedAttemptCount { get; private set; }

    public static Credential Create(
        Guid principalExternalId,
        Guid tenantExternalId,
        CredentialType credentialType,
        string verifier,
        string? salt,
        DateTimeOffset? expiresAt,
        string createdBy)
    {
        if (expiresAt.HasValue && expiresAt.Value <= DateTimeOffset.UtcNow)
            throw new DomainException("Credential expiry must be in the future.");

        var entity = new Credential
        {
            CredentialExternalId = Guid.NewGuid(),
            PrincipalExternalId = Guard.AgainstDefault(principalExternalId, nameof(principalExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            CredentialType = Guard.AgainstInvalidEnum(credentialType, nameof(credentialType)),
            Status = CredentialStatus.Active,
            Verifier = Guard.AgainstNullOrWhiteSpace(verifier, nameof(verifier)),
            Salt = string.IsNullOrWhiteSpace(salt) ? null : salt.Trim(),
            Version = 1,
            ExpiresAt = expiresAt,
            FailedAttemptCount = 0
        };

        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void RegisterSuccessfulUse(DateTimeOffset usedAt, string updatedBy)
    {
        EnsureUsable();
        LastUsedAt = usedAt;
        FailedAttemptCount = 0;

        if (Status == CredentialStatus.Locked)
        {
            Status = CredentialStatus.Active;
            LockoutEndAt = null;
        }

        Touch(updatedBy);
    }

    public void RegisterFailedAttempt(int maxAttempts, TimeSpan lockoutWindow, string updatedBy)
    {
        EnsureNotTerminal();

        FailedAttemptCount++;

        if (FailedAttemptCount >= Guard.AgainstNegative(maxAttempts, nameof(maxAttempts)))
        {
            Status = CredentialStatus.Locked;
            LockoutEndAt = DateTimeOffset.UtcNow.Add(lockoutWindow);
        }

        Touch(updatedBy);
    }

    public void Unlock(string updatedBy)
    {
        if (Status != CredentialStatus.Locked)
            throw new DomainException("Only locked credentials can be unlocked.");

        Status = CredentialStatus.Active;
        FailedAttemptCount = 0;
        LockoutEndAt = null;
        Touch(updatedBy);
    }

    public void Rotate(string verifier, string? salt, DateTimeOffset? expiresAt, string updatedBy)
    {
        EnsureNotTerminal();

        if (expiresAt.HasValue && expiresAt.Value <= DateTimeOffset.UtcNow)
            throw new DomainException("Credential expiry must be in the future.");

        Verifier = Guard.AgainstNullOrWhiteSpace(verifier, nameof(verifier));
        Salt = string.IsNullOrWhiteSpace(salt) ? null : salt.Trim();
        ExpiresAt = expiresAt;
        FailedAttemptCount = 0;
        LockoutEndAt = null;
        Status = CredentialStatus.Active;
        Version++;
        Touch(updatedBy);
    }

    public void MarkCompromised(string updatedBy)
    {
        EnsureNotTerminal();
        Status = CredentialStatus.Compromised;
        Touch(updatedBy);
    }

    public void Revoke(string updatedBy)
    {
        if (Status == CredentialStatus.Revoked)
            throw new DomainException("Credential is already revoked.");

        Status = CredentialStatus.Revoked;
        RevokedAt = DateTimeOffset.UtcNow;
        Touch(updatedBy);
    }

    public void Expire(string updatedBy)
    {
        if (Status == CredentialStatus.Revoked)
            throw new DomainException("Revoked credentials cannot transition to expired.");

        Status = CredentialStatus.Expired;
        Touch(updatedBy);
    }

    public bool IsCurrentlyLocked()
    {
        if (Status != CredentialStatus.Locked)
            return false;

        return !LockoutEndAt.HasValue || LockoutEndAt.Value > DateTimeOffset.UtcNow;
    }

    public bool IsUsable()
    {
        if (Status is CredentialStatus.Revoked or CredentialStatus.Compromised or CredentialStatus.Expired)
            return false;

        if (Status == CredentialStatus.Locked && IsCurrentlyLocked())
            return false;

        if (ExpiresAt.HasValue && ExpiresAt.Value <= DateTimeOffset.UtcNow)
            return false;

        return true;
    }

    private void EnsureUsable()
    {
        if (!IsUsable())
            throw new DomainException("Credential is not usable.");
    }

    private void EnsureNotTerminal()
    {
        if (Status is CredentialStatus.Revoked or CredentialStatus.Compromised)
            throw new DomainException("Credential is in a terminal state.");
    }
}