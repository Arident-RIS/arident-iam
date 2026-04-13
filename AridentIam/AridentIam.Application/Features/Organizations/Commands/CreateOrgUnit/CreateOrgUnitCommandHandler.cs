using AridentIam.Application.Common.CQRS;
using AridentIam.Application.Common.Exceptions;
using AridentIam.Application.Common.Interfaces;
using AridentIam.Domain.Entities.Organizations;
using AridentIam.Domain.Interfaces.Repositories;

namespace AridentIam.Application.Features.Organizations.Commands.CreateOrgUnit;

public sealed class CreateOrgUnitCommandHandler(
    ITenantRepository tenantRepository,
    IOrganizationRepository organizationRepository,
    ICurrentUserService currentUser)
    : ICommandHandler<CreateOrgUnitCommand, CreateOrgUnitResponse>
{
    public async Task<CreateOrgUnitResponse> Handle(
        CreateOrgUnitCommand request,
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

        var schema = await organizationRepository.GetOrgSchemaByExternalIdAsync(
            request.OrgSchemaExternalId,
            cancellationToken);

        if (schema is null)
        {
            throw new NotFoundException(
                $"Organization schema with ExternalId '{request.OrgSchemaExternalId}' was not found.");
        }

        var unitType = await organizationRepository.GetOrgUnitTypeByExternalIdAsync(
            request.OrgUnitTypeExternalId,
            cancellationToken);

        if (unitType is null)
        {
            throw new NotFoundException(
                $"Organization unit type with ExternalId '{request.OrgUnitTypeExternalId}' was not found.");
        }

        if (request.ParentOrganizationUnitExternalId.HasValue)
        {
            var parent = await organizationRepository.GetOrgUnitByExternalIdAsync(
                request.ParentOrganizationUnitExternalId.Value,
                cancellationToken);

            if (parent is null)
            {
                throw new NotFoundException(
                    $"Parent organization unit with ExternalId '{request.ParentOrganizationUnitExternalId}' was not found.");
            }
        }

        var unitExists = await organizationRepository.ExistsOrgUnitByCodeAsync(
            request.TenantExternalId,
            request.OrgSchemaExternalId,
            request.Code,
            cancellationToken);

        if (unitExists)
        {
            throw new ConflictException(
                $"An organization unit with code '{request.Code}' already exists for this schema.");
        }

        var path = request.ParentOrganizationUnitExternalId.HasValue
            ? $"{request.ParentOrganizationUnitExternalId.Value}/{request.Code}"
            : request.Code;

        var depth = request.ParentOrganizationUnitExternalId.HasValue ? 1 : 0;

        var orgUnit = OrgUnit.Create(
            tenantExternalId: request.TenantExternalId,
            orgSchemaExternalId: request.OrgSchemaExternalId,
            orgUnitTypeExternalId: request.OrgUnitTypeExternalId,
            parentOrganizationUnitExternalId: request.ParentOrganizationUnitExternalId,
            code: request.Code,
            name: request.Name,
            path: path,
            depth: depth,
            createdBy: currentUser.ActorIdentifier);

        await organizationRepository.AddOrgUnitAsync(orgUnit, cancellationToken);

        return new CreateOrgUnitResponse(
            OrganizationUnitExternalId: orgUnit.OrganizationUnitExternalId,
            OrgSchemaExternalId: orgUnit.OrgSchemaExternalId,
            Code: orgUnit.Code,
            Name: orgUnit.Name);
    }
}