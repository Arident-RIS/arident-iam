using AridentIam.Application.Common.Exceptions;
using AridentIam.Application.Features.Tenants.DTOs;
using AridentIam.Application.Features.Tenants.Mappings;
using AridentIam.Domain.Interfaces.Repositories;
using MediatR;

namespace AridentIam.Application.Features.Tenants.Queries.GetTenantByExternalId;

public sealed class GetTenantByExternalIdQueryHandler(
    ITenantRepository tenantRepository)
    : IRequestHandler<GetTenantByExternalIdQuery, TenantDto>
{
    public async Task<TenantDto> Handle(
        GetTenantByExternalIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByExternalIdAsync(
            request.TenantExternalId,
            cancellationToken);

        if (tenant is null)
        {
            throw new NotFoundException(
                $"Tenant with ExternalId '{request.TenantExternalId}' was not found.");
        }

        return tenant.ToDto();
    }
}