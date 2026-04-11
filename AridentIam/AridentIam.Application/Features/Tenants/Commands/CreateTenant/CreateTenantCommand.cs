using AridentIam.Application.Common.CQRS;
using AridentIam.Domain.Enums;

namespace AridentIam.Application.Features.Tenants.Commands.CreateTenant;

public sealed record CreateTenantCommand(
    string Code,
    string Name,
    IsolationMode IsolationMode,
    string? DefaultLocale,
    string? DefaultTimeZone)
    : ICommand<CreateTenantResponse>;