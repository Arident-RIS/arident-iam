using AridentIam.Application.Common.CQRS;
using AridentIam.Application.Features.Organizations.DTOs;

namespace AridentIam.Application.Features.Organizations.Queries.GetOrgUnitByExternalId;

public sealed record GetOrgUnitByExternalIdQuery(Guid OrganizationUnitExternalId)
    : IQuery<OrgUnitDto>;