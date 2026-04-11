using AridentIam.Application.Common.Exceptions;
using AridentIam.Application.Features.Users.DTOs;
using AridentIam.Application.Features.Users.Mappings;
using AridentIam.Domain.Interfaces.Repositories;
using MediatR;

namespace AridentIam.Application.Features.Users.Queries.GetUserByExternalId;

public sealed class GetUserByExternalIdQueryHandler(
    IUserRepository userRepository)
    : IRequestHandler<GetUserByExternalIdQuery, UserDto>
{
    public async Task<UserDto> Handle(
        GetUserByExternalIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByExternalIdAsync(
            request.UserExternalId,
            cancellationToken);

        return user is null
            ? throw new NotFoundException(
                $"User with ExternalId '{request.UserExternalId}' was not found.")
            : user.ToDto();
    }
}