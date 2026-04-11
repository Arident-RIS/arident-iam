using AridentIam.Domain.Enums;

namespace AridentIam.WebApi.Contracts.Users;

/// <summary>
/// Request body for creating a user within a tenant.
/// </summary>
public sealed record CreateUserRequest(
    Guid TenantExternalId,
    string FirstName,
    string LastName,
    string DisplayName,
    string Email,
    string Username,
    string PhoneNumber,
    string? JobTitle,
    EmploymentType EmploymentType);