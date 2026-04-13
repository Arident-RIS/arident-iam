using AridentIam.Application.Common.CQRS;
using AridentIam.Application.Common.Exceptions;
using AridentIam.Application.Common.Interfaces;
using AridentIam.Domain.Entities.Organizations;
using AridentIam.Domain.Interfaces.Repositories;

namespace AridentIam.Application.Features.Organizations.Commands.CreateOrgSchema;

public sealed class CreateOrgSchemaCommandHandler(
    ITenantRepository tenantRepository,
    IOrganizationRepository organizationRepository,
    ICurrentUserService currentUser)
    : ICommandHandler<CreateOrgSchemaCommand, CreateOrgSchemaResponse>
{
    public async Task<CreateOrgSchemaResponse> Handle(
        CreateOrgSchemaCommand request,
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

        var schemaExists = await organizationRepository.ExistsOrgSchemaByNameAsync(
            request.TenantExternalId,
            request.Name,
            cancellationToken);

        if (schemaExists)
        {
            throw new ConflictException(
                $"An organization schema named '{request.Name}' already exists in this tenant.");
        }

        var orgSchema = OrgSchema.Create(
            tenantExternalId: request.TenantExternalId,
            name: request.Name,
            description: request.Description,
            isDefault: request.IsDefault,
            createdBy: currentUser.ActorIdentifier);

        await organizationRepository.AddOrgSchemaAsync(orgSchema, cancellationToken);

        return new CreateOrgSchemaResponse(
            OrgSchemaExternalId: orgSchema.OrgSchemaExternalId,
            TenantExternalId: orgSchema.TenantExternalId,
            Name: orgSchema.Name);
    }
}