using AridentIam.Domain.Enums;

namespace AridentIam.Application.Features.Organizations.DTOs;

public sealed record OrgUnitDto(
    Guid OrganizationUnitExternalId,
    Guid TenantExternalId,
    Guid OrgSchemaExternalId,
    Guid OrgUnitTypeExternalId,
    Guid? ParentOrganizationUnitExternalId,
    string Code,
    string Name,
    string Path,
    int Depth,
    RecordStatus Status);