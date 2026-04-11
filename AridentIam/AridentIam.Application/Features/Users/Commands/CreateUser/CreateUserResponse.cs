namespace AridentIam.Application.Features.Users.Commands.CreateUser;

public sealed record CreateUserResponse(
    Guid PrincipalExternalId,
    Guid UserExternalId,
    Guid TenantExternalId,
    string DisplayName,
    string Email,
    string Username);