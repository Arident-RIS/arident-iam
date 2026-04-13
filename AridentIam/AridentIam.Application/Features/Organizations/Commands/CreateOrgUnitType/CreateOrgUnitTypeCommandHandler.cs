using AridentIam.Application.Common.CQRS;
using AridentIam.Application.Common.Exceptions;
using AridentIam.Application.Common.Interfaces;
using AridentIam.Domain.Entities.Organizations;
using AridentIam.Domain.Interfaces.Repositories;

namespace AridentIam.Application.Features.Organizations.Commands.CreateOrgUnitType;

public sealed class CreateOrgUnitTypeCommandHandler(
    ITenantRepository tenantRepository,
    IOrganizationRepository organizationRepository,
    ICurrentUserService currentUser)
    : ICommandHandler<CreateOrgUnitTypeCommand, CreateOrgUnitTypeResponse>
{
    public async Task<CreateOrgUnitTypeResponse> Handle(
        CreateOrgUnitTypeCommand request,
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

        var unitTypeExists = await organizationRepository.ExistsOrgUnitTypeByCodeAsync(
            request.TenantExternalId,
            request.OrgSchemaExternalId,
            request.Code,
            cancellationToken);

        if (unitTypeExists)
        {
            throw new ConflictException(
                $"An organization unit type with code '{request.Code}' already exists for this schema.");
        }

        var orgUnitType = OrgUnitType.Create(
            tenantExternalId: request.TenantExternalId,
            orgSchemaExternalId: request.OrgSchemaExternalId,
            code: request.Code,
            name: request.Name,
            hierarchyLevel: request.HierarchyLevel,
            createdBy: currentUser.ActorIdentifier);

        await organizationRepository.AddOrgUnitTypeAsync(orgUnitType, cancellationToken);

        return new CreateOrgUnitTypeResponse(
            OrgUnitTypeExternalId: orgUnitType.OrgUnitTypeExternalId,
            OrgSchemaExternalId: orgUnitType.OrgSchemaExternalId,
            Code: orgUnitType.Code,
            Name: orgUnitType.Name);
    }
}