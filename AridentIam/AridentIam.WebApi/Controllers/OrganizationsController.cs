using AridentIam.Application.Features.Organizations.Commands.CreateOrgSchema;
using AridentIam.Application.Features.Organizations.Commands.CreateOrgUnit;
using AridentIam.Application.Features.Organizations.Commands.CreateOrgUnitType;
using AridentIam.Application.Features.Organizations.DTOs;
using AridentIam.Application.Features.Organizations.Queries.GetOrgUnitByExternalId;
using AridentIam.WebApi.Contracts.Organizations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AridentIam.WebApi.Controllers;

[ApiController]
[Route("api/v1/organizations")]
[Produces("application/json")]
public sealed class OrganizationsController(IMediator mediator) : ControllerBase
{
    [HttpPost("schemas")]
    [ProducesResponseType(typeof(CreateOrgSchemaResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateOrgSchema(
        [FromBody] CreateOrgSchemaRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateOrgSchemaCommand(
            TenantExternalId: request.TenantExternalId,
            Name: request.Name,
            Description: request.Description,
            IsDefault: request.IsDefault);

        var result = await mediator.Send(command, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("unit-types")]
    [ProducesResponseType(typeof(CreateOrgUnitTypeResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateOrgUnitType(
        [FromBody] CreateOrgUnitTypeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateOrgUnitTypeCommand(
            TenantExternalId: request.TenantExternalId,
            OrgSchemaExternalId: request.OrgSchemaExternalId,
            Code: request.Code,
            Name: request.Name,
            HierarchyLevel: request.HierarchyLevel);

        var result = await mediator.Send(command, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("units")]
    [ProducesResponseType(typeof(CreateOrgUnitResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateOrgUnit(
        [FromBody] CreateOrgUnitRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateOrgUnitCommand(
            TenantExternalId: request.TenantExternalId,
            OrgSchemaExternalId: request.OrgSchemaExternalId,
            OrgUnitTypeExternalId: request.OrgUnitTypeExternalId,
            ParentOrganizationUnitExternalId: request.ParentOrganizationUnitExternalId,
            Code: request.Code,
            Name: request.Name);

        var result = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetOrgUnitByExternalId),
            new { organizationUnitExternalId = result.OrganizationUnitExternalId },
            result);
    }

    [HttpGet("units/{organizationUnitExternalId:guid}")]
    [ProducesResponseType(typeof(OrgUnitDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrgUnitByExternalId(
        Guid organizationUnitExternalId,
        CancellationToken cancellationToken)
    {
        var query = new GetOrgUnitByExternalIdQuery(organizationUnitExternalId);
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}