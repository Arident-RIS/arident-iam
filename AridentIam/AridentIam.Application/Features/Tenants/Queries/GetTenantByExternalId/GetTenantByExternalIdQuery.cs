using AridentIam.Application.Common.CQRS;
using AridentIam.Application.Features.Tenants.DTOs;

namespace AridentIam.Application.Features.Tenants.Queries.GetTenantByExternalId;

public sealed record GetTenantByExternalIdQuery(Guid TenantExternalId)
    : IQuery<TenantDto>;