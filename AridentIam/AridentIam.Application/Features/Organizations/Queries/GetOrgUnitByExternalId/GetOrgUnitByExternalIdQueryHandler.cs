using AridentIam.Application.Common.CQRS;
using AridentIam.Application.Common.Exceptions;
using AridentIam.Application.Features.Organizations.DTOs;
using AridentIam.Application.Features.Organizations.Mappings;
using AridentIam.Domain.Interfaces.Repositories;

namespace AridentIam.Application.Features.Organizations.Queries.GetOrgUnitByExternalId;

public sealed class GetOrgUnitByExternalIdQueryHandler(
    IOrganizationRepository organizationRepository)
    : IQueryHandler<GetOrgUnitByExternalIdQuery, OrgUnitDto>
{
    public async Task<OrgUnitDto> Handle(
        GetOrgUnitByExternalIdQuery request,
        CancellationToken cancellationToken)
    {
        var orgUnit = await organizationRepository.GetOrgUnitByExternalIdAsync(
            request.OrganizationUnitExternalId,
            cancellationToken);

        if (orgUnit is null)
        {
            throw new NotFoundException(
                $"Organization unit with ExternalId '{request.OrganizationUnitExternalId}' was not found.");
        }

        return orgUnit.ToDto();
    }
}