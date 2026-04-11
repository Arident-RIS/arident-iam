using AridentIam.Application.Features.Users.DTOs;
using AridentIam.Domain.Entities.Users;

namespace AridentIam.Application.Features.Users.Mappings;

public static class UserMappings
{
    public static UserDto ToDto(this User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new UserDto(
            UserExternalId: user.UserExternalId,
            PrincipalExternalId: user.PrincipalExternalId,
            TenantExternalId: user.TenantExternalId,
            FirstName: user.FirstName,
            LastName: user.LastName,
            DisplayName: user.DisplayName,
            Email: user.Email,
            Username: user.Username,
            PhoneNumber: user.PhoneNumber,
            EmploymentType: user.EmploymentType,
            JobTitle: user.JobTitle,
            IsEmailVerified: user.IsEmailVerified,
            IsPhoneVerified: user.IsPhoneVerified);
    }
}