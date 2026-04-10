using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;
using AridentIam.Domain.Events;
using AridentIam.Domain.ValueObjects;

namespace AridentIam.Domain.Entities.Users;

public sealed class User : AggregateRoot
{
    private User() { }
    public Guid UserExternalId { get; private set; }
    public Guid PrincipalExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string DisplayName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Username { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public EmploymentType EmploymentType { get; private set; } = EmploymentType.Unknown;
    public string? JobTitle { get; private set; }
    public UserStatus Status { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public bool IsPhoneVerified { get; private set; }
    public bool IsLocked { get; private set; }
    public DateTimeOffset? LockoutEndAt { get; private set; }
    public DateTimeOffset? LastLoginAt { get; private set; }
    public string? PasswordHash { get; private set; }
    public string? PasswordSalt { get; private set; }

    public static User Create(Guid tenantExternalId, string firstName, string lastName, string displayName, string email, string username, string phoneNumber, string createdBy)
    {
        var personName = new PersonName(firstName, lastName);
        var emailValue = new EmailAddress(email);
        var usernameValue = new Username(username);
        var phoneValue = new PhoneNumber(phoneNumber);

        var entity = new User
        {
            UserExternalId = Guid.NewGuid(),
            PrincipalExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            FirstName = personName.FirstName,
            LastName = personName.LastName,
            DisplayName = Guard.AgainstNullOrWhiteSpace(displayName, nameof(displayName)),
            Email = emailValue.Value,
            Username = usernameValue.Value,
            PhoneNumber = phoneValue.Value,
            Status = UserStatus.PendingActivation
        };

        entity.SetCreationAudit(createdBy);
        entity.RaiseDomainEvent(new UserCreatedDomainEvent(entity.UserExternalId, entity.TenantExternalId));
        return entity;
    }

    public void SetPasswordHash(string passwordHash, string passwordSalt, string updatedBy)
    {
        PasswordHash = Guard.AgainstNullOrWhiteSpace(passwordHash, nameof(passwordHash));
        PasswordSalt = Guard.AgainstNullOrWhiteSpace(passwordSalt, nameof(passwordSalt));
        Touch(updatedBy);
    }

    public void Activate(string updatedBy) { Status = UserStatus.Active; Touch(updatedBy); }
    public void Deactivate(string updatedBy) { Status = UserStatus.Inactive; Touch(updatedBy); }

    public void VerifyEmail(string updatedBy)
    {
        IsEmailVerified = true;
        Touch(updatedBy);
        RaiseDomainEvent(new UserEmailVerifiedDomainEvent(UserExternalId));
    }

    public void VerifyPhone(string updatedBy) { IsPhoneVerified = true; Touch(updatedBy); }

    public void Lock(DateTimeOffset? lockoutEndAt, string updatedBy)
    {
        IsLocked = true;
        LockoutEndAt = lockoutEndAt;
        Status = UserStatus.Locked;
        Touch(updatedBy);
    }

    public void Unlock(string updatedBy)
    {
        IsLocked = false;
        LockoutEndAt = null;
        Status = UserStatus.Active;
        Touch(updatedBy);
    }

    public void UpdateProfile(string firstName, string lastName, string displayName, string phoneNumber, string? jobTitle, EmploymentType employmentType, string updatedBy)
    {
        var personName = new PersonName(firstName, lastName);
        var phoneValue = new PhoneNumber(phoneNumber);
        FirstName = personName.FirstName;
        LastName = personName.LastName;
        DisplayName = Guard.AgainstNullOrWhiteSpace(displayName, nameof(displayName));
        PhoneNumber = phoneValue.Value;
        JobTitle = string.IsNullOrWhiteSpace(jobTitle) ? null : jobTitle.Trim();
        EmploymentType = employmentType;
        Touch(updatedBy);
    }

    public void MarkLogin(DateTimeOffset loginAt, string updatedBy) { LastLoginAt = loginAt; Touch(updatedBy); }
}
