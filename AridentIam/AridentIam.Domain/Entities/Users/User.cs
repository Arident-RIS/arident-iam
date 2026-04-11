using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;
using AridentIam.Domain.ValueObjects;

namespace AridentIam.Domain.Entities.Users;

public sealed class User : AuditableEntity
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

    public bool IsEmailVerified { get; private set; }
    public bool IsPhoneVerified { get; private set; }

    public static User Create(
        Guid principalExternalId,
        Guid tenantExternalId,
        string firstName,
        string lastName,
        string displayName,
        string email,
        string username,
        string phoneNumber,
        string? jobTitle,
        EmploymentType employmentType,
        string createdBy)
    {
        var personName = new PersonName(firstName, lastName);
        var emailAddress = new EmailAddress(email);
        var usernameValue = new Username(username);
        var phoneValue = new PhoneNumber(phoneNumber);

        var entity = new User
        {
            UserExternalId = Guid.NewGuid(),
            PrincipalExternalId = Guard.AgainstDefault(principalExternalId, nameof(principalExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            FirstName = personName.FirstName,
            LastName = personName.LastName,
            DisplayName = Guard.AgainstMaxLength(displayName, 200, nameof(displayName)),
            Email = emailAddress.Value,
            Username = usernameValue.Value,
            PhoneNumber = phoneValue.Value,
            JobTitle = string.IsNullOrWhiteSpace(jobTitle) ? null : Guard.AgainstMaxLength(jobTitle, 150, nameof(jobTitle)),
            EmploymentType = Guard.AgainstInvalidEnum(employmentType, nameof(employmentType)),
            IsEmailVerified = false,
            IsPhoneVerified = false
        };

        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void UpdateProfile(
        string firstName,
        string lastName,
        string displayName,
        string phoneNumber,
        string? jobTitle,
        EmploymentType employmentType,
        string updatedBy)
    {
        var personName = new PersonName(firstName, lastName);
        var phoneValue = new PhoneNumber(phoneNumber);

        FirstName = personName.FirstName;
        LastName = personName.LastName;
        DisplayName = Guard.AgainstMaxLength(displayName, 200, nameof(displayName));
        PhoneNumber = phoneValue.Value;
        JobTitle = string.IsNullOrWhiteSpace(jobTitle) ? null : Guard.AgainstMaxLength(jobTitle, 150, nameof(jobTitle));
        EmploymentType = Guard.AgainstInvalidEnum(employmentType, nameof(employmentType));

        Touch(updatedBy);
    }

    public void ChangeEmail(string email, string updatedBy)
    {
        var emailAddress = new EmailAddress(email);
        Email = emailAddress.Value;
        IsEmailVerified = false;
        Touch(updatedBy);
    }

    public void ChangeUsername(string username, string updatedBy)
    {
        var usernameValue = new Username(username);
        Username = usernameValue.Value;
        Touch(updatedBy);
    }

    public void VerifyEmail(string updatedBy)
    {
        if (IsEmailVerified)
            throw new DomainException("Email is already verified.");

        IsEmailVerified = true;
        Touch(updatedBy);
    }

    public void VerifyPhone(string updatedBy)
    {
        if (IsPhoneVerified)
            throw new DomainException("Phone number is already verified.");

        IsPhoneVerified = true;
        Touch(updatedBy);
    }
}