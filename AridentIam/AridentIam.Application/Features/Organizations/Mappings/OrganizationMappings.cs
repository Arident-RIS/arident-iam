using AridentIam.Application.Features.Organizations.DTOs;
using AridentIam.Domain.Entities.Organizations;

namespace AridentIam.Application.Features.Organizations.Mappings;

public static class OrganizationMappings
{
    public static OrgUnitDto ToDto(this OrgUnit orgUnit)
    {
        ArgumentNullException.ThrowIfNull(orgUnit);

        return new OrgUnitDto(
            OrganizationUnitExternalId: orgUnit.OrganizationUnitExternalId,
            TenantExternalId: orgUnit.TenantExternalId,
            OrgSchemaExternalId: orgUnit.OrgSchemaExternalId,
            OrgUnitTypeExternalId: orgUnit.OrgUnitTypeExternalId,
            ParentOrganizationUnitExternalId: orgUnit.ParentOrganizationUnitExternalId,
            Code: orgUnit.Code,
            Name: orgUnit.Name,
            Path: orgUnit.Path,
            Depth: orgUnit.Depth,
            Status: orgUnit.Status);
    }
}