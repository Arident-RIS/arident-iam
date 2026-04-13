using AridentIam.Application.Common.CQRS;

namespace AridentIam.Application.Features.Organizations.Commands.CreateOrgSchema;

public sealed record CreateOrgSchemaCommand(
    Guid TenantExternalId,
    string Name,
    string? Description,
    bool IsDefault)
    : ICommand<CreateOrgSchemaResponse>;