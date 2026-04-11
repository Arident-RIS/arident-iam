using AridentIam.Domain.Common;
using AridentIam.Domain.Entities.Users;
using AridentIam.Domain.Enums;
using AridentIam.Domain.Events;

namespace AridentIam.Domain.Entities.Principals;

public sealed class Principal : AggregateRoot
{
    private Principal() { }

    public Guid PrincipalExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public PrincipalType PrincipalType { get; private set; }
    public string DisplayName { get; private set; } = null!;
    public string? ExternalReference { get; private set; }
    public PrincipalStatus Status { get; private set; }
    public LifecycleState LifecycleState { get; private set; }

    public User? UserProfile { get; private set; }

    public static Principal Create(
        Guid tenantExternalId,
        PrincipalType principalType,
        string displayName,
        string? externalReference,
        string createdBy)
    {
        var entity = new Principal
        {
            PrincipalExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            PrincipalType = Guard.AgainstInvalidEnum(principalType, nameof(principalType)),
            DisplayName = Guard.AgainstMaxLength(displayName, 200, nameof(displayName)),
            ExternalReference = string.IsNullOrWhiteSpace(externalReference) ? null : externalReference.Trim(),
            Status = PrincipalStatus.Active,
            LifecycleState = LifecycleState.Active
        };

        entity.SetCreationAudit(createdBy);
        entity.RaiseDomainEvent(new PrincipalCreatedDomainEvent(entity.PrincipalExternalId, entity.TenantExternalId));
        return entity;
    }

    public void Rename(string displayName, string updatedBy)
    {
        EnsureMutable();
        DisplayName = Guard.AgainstMaxLength(displayName, 200, nameof(displayName));
        Touch(updatedBy);
    }

    public void SetExternalReference(string? externalReference, string updatedBy)
    {
        EnsureMutable();
        ExternalReference = string.IsNullOrWhiteSpace(externalReference) ? null : externalReference.Trim();
        Touch(updatedBy);
    }

    public void Suspend(string updatedBy)
    {
        EnsureNotDeleted();
        Status = PrincipalStatus.Suspended;
        LifecycleState = LifecycleState.Inactive;
        Touch(updatedBy);
    }

    public void Activate(string updatedBy)
    {
        EnsureNotDeleted();
        Status = PrincipalStatus.Active;
        LifecycleState = LifecycleState.Active;
        Touch(updatedBy);
    }

    public void Disable(string updatedBy)
    {
        EnsureNotDeleted();
        Status = PrincipalStatus.Inactive;
        LifecycleState = LifecycleState.Inactive;
        Touch(updatedBy);
    }

    public void Delete(string updatedBy)
    {
        Status = PrincipalStatus.Deleted;
        LifecycleState = LifecycleState.Deleted;
        Touch(updatedBy);
    }

    public void AttachUserProfile(
        string firstName,
        string lastName,
        string email,
        string username,
        string phoneNumber,
        string? jobTitle,
        EmploymentType employmentType,
        string createdBy)
    {
        if (PrincipalType != PrincipalType.User)
            throw new DomainException("Only user principals can own a user profile.");

        if (UserProfile is not null)
            throw new DomainException("User profile is already attached to this principal.");

        UserProfile = User.Create(
            principalExternalId: PrincipalExternalId,
            tenantExternalId: TenantExternalId,
            firstName: firstName,
            lastName: lastName,
            displayName: DisplayName,
            email: email,
            username: username,
            phoneNumber: phoneNumber,
            jobTitle: jobTitle,
            employmentType: employmentType,
            createdBy: createdBy);

        Touch(createdBy);
    }

    public void UpdateUserProfile(
        string firstName,
        string lastName,
        string displayName,
        string phoneNumber,
        string? jobTitle,
        EmploymentType employmentType,
        string updatedBy)
    {
        EnsureUserProfileExists();
        Rename(displayName, updatedBy);
        UserProfile!.UpdateProfile(firstName, lastName, displayName, phoneNumber, jobTitle, employmentType, updatedBy);
        Touch(updatedBy);
    }

    public void ChangeUserEmail(string email, string updatedBy)
    {
        EnsureUserProfileExists();
        UserProfile!.ChangeEmail(email, updatedBy);
        Touch(updatedBy);
    }

    public void ChangeUsername(string username, string updatedBy)
    {
        EnsureUserProfileExists();
        UserProfile!.ChangeUsername(username, updatedBy);
        Touch(updatedBy);
    }

    public void VerifyUserEmail(string updatedBy)
    {
        EnsureUserProfileExists();
        UserProfile!.VerifyEmail(updatedBy);
        Touch(updatedBy);
        RaiseDomainEvent(new PrincipalEmailVerifiedDomainEvent(PrincipalExternalId, TenantExternalId));
    }

    public void VerifyUserPhone(string updatedBy)
    {
        EnsureUserProfileExists();
        UserProfile!.VerifyPhone(updatedBy);
        Touch(updatedBy);
    }

    private void EnsureUserProfileExists()
    {
        if (UserProfile is null)
            throw new DomainException("User profile is not attached to this principal.");
    }

    private void EnsureMutable()
    {
        EnsureNotDeleted();

        if (Status == PrincipalStatus.Suspended)
            throw new DomainException("Suspended principals cannot be modified.");
    }

    private void EnsureNotDeleted()
    {
        if (Status == PrincipalStatus.Deleted || LifecycleState == LifecycleState.Deleted)
            throw new DomainException("Deleted principals cannot be modified.");
    }
}