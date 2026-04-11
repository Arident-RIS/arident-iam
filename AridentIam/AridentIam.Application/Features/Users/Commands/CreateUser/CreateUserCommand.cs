using AridentIam.Application.Common.CQRS;
using AridentIam.Domain.Enums;

namespace AridentIam.Application.Features.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    Guid TenantExternalId,
    string FirstName,
    string LastName,
    string DisplayName,
    string Email,
    string Username,
    string PhoneNumber,
    string? JobTitle,
    EmploymentType EmploymentType) : ICommand<CreateUserResponse>;