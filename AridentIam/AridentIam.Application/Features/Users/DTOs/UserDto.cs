using AridentIam.Domain.Enums;

namespace AridentIam.Application.Features.Users.DTOs;

public sealed record UserDto(
    Guid UserExternalId,
    Guid PrincipalExternalId,
    Guid TenantExternalId,
    string FirstName,
    string LastName,
    string DisplayName,
    string Email,
    string Username,
    string PhoneNumber,
    EmploymentType EmploymentType,
    string? JobTitle,
    bool IsEmailVerified,
    bool IsPhoneVerified);