using AridentIam.Application.Common.Exceptions;
using AridentIam.Domain.Entities.Tenants;
using AridentIam.Domain.Interfaces.Repositories;
using MediatR;

namespace AridentIam.Application.Features.Tenants.Commands.CreateTenant;

public sealed class CreateTenantCommandHandler(
    ITenantRepository tenantRepository)
    : IRequestHandler<CreateTenantCommand, CreateTenantResponse>
{
    private const string SystemActor = "system";

    public async Task<CreateTenantResponse> Handle(
        CreateTenantCommand request,
        CancellationToken cancellationToken)
    {
        var codeExists = await tenantRepository.ExistsByCodeAsync(
            request.Code,
            cancellationToken);

        if (codeExists)
        {
            throw new ConflictException(
                $"A tenant with code '{request.Code}' already exists.");
        }

        var tenant = Tenant.Create(
            tenantExternalId: Guid.NewGuid(),
            code: request.Code,
            name: request.Name,
            isolationMode: request.IsolationMode,
            createdBy: SystemActor);

        if (!string.IsNullOrWhiteSpace(request.DefaultLocale))
        {
            tenant.SetLocale(request.DefaultLocale, SystemActor);
        }

        if (!string.IsNullOrWhiteSpace(request.DefaultTimeZone))
        {
            tenant.SetTimeZone(request.DefaultTimeZone, SystemActor);
        }

        await tenantRepository.AddAsync(tenant, cancellationToken);

        return new CreateTenantResponse(
            TenantExternalId: tenant.TenantExternalId,
            Code: tenant.Code,
            Name: tenant.Name);
    }
}