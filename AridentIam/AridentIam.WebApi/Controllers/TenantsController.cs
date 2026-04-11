using AridentIam.Application.Features.Tenants.Commands.CreateTenant;
using AridentIam.Application.Features.Tenants.DTOs;
using AridentIam.Application.Features.Tenants.Queries.GetTenantByExternalId;
using AridentIam.WebApi.Contracts.Tenants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AridentIam.WebApi.Controllers;

[ApiController]
[Route("api/v1/tenants")]
[Produces("application/json")]
public sealed class TenantsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateTenantResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateTenant(
        [FromBody] CreateTenantRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateTenantCommand(
            Code: request.Code,
            Name: request.Name,
            IsolationMode: request.IsolationMode,
            DefaultLocale: request.DefaultLocale,
            DefaultTimeZone: request.DefaultTimeZone);

        var result = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            actionName: nameof(GetTenantByExternalId),
            routeValues: new { tenantExternalId = result.TenantExternalId },
            value: result);
    }

    [HttpGet("{tenantExternalId:guid}")]
    [ProducesResponseType(typeof(TenantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTenantByExternalId(
        Guid tenantExternalId,
        CancellationToken cancellationToken)
    {
        var query = new GetTenantByExternalIdQuery(tenantExternalId);
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}