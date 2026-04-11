using AridentIam.Application.Common.CQRS;
using AridentIam.Application.Features.Users.DTOs;

namespace AridentIam.Application.Features.Users.Queries.GetUserByExternalId;

public sealed record GetUserByExternalIdQuery(Guid UserExternalId)
    : IQuery<UserDto>;