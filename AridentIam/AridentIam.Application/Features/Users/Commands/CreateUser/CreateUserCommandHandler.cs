using AridentIam.Application.Common.CQRS;
using AridentIam.Application.Common.Exceptions;
using AridentIam.Application.Common.Interfaces;
using AridentIam.Domain.Entities.Principals;
using AridentIam.Domain.Enums;
using AridentIam.Domain.Interfaces.Repositories;

namespace AridentIam.Application.Features.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler(
    ITenantRepository tenantRepository,
    IPrincipalRepository principalRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUser)
    : ICommandHandler<CreateUserCommand, CreateUserResponse>
{
    public async Task<CreateUserResponse> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var tenantExists = await tenantRepository.ExistsByExternalIdAsync(
            request.TenantExternalId,
            cancellationToken);

        if (!tenantExists)
        {
            throw new NotFoundException(
                $"Tenant with ExternalId '{request.TenantExternalId}' was not found.");
        }

        var emailExists = await userRepository.ExistsByEmailAsync(
            request.TenantExternalId,
            request.Email,
            cancellationToken);

        if (emailExists)
        {
            throw new ConflictException(
                $"A user with email '{request.Email}' already exists in this tenant.");
        }

        var usernameExists = await userRepository.ExistsByUsernameAsync(
            request.TenantExternalId,
            request.Username,
            cancellationToken);

        if (usernameExists)
        {
            throw new ConflictException(
                $"A user with username '{request.Username}' already exists in this tenant.");
        }

        var actor = currentUser.ActorIdentifier;

        var principal = Principal.Create(
            tenantExternalId: request.TenantExternalId,
            principalType: PrincipalType.User,
            displayName: request.DisplayName,
            externalReference: null,
            createdBy: actor);

        principal.AttachUserProfile(
            firstName: request.FirstName,
            lastName: request.LastName,
            email: request.Email,
            username: request.Username,
            phoneNumber: request.PhoneNumber,
            jobTitle: request.JobTitle,
            employmentType: request.EmploymentType,
            createdBy: actor);

        await principalRepository.AddAsync(principal, cancellationToken);

        var userProfile = principal.UserProfile
            ?? throw new InvalidOperationException("User profile was not created for the principal.");

        return new CreateUserResponse(
            PrincipalExternalId: principal.PrincipalExternalId,
            UserExternalId: userProfile.UserExternalId,
            TenantExternalId: principal.TenantExternalId,
            DisplayName: principal.DisplayName,
            Email: userProfile.Email,
            Username: userProfile.Username);
    }
}