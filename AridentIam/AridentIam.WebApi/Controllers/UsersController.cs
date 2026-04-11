using AridentIam.Application.Features.Users.Commands.CreateUser;
using AridentIam.Application.Features.Users.DTOs;
using AridentIam.Application.Features.Users.Queries.GetUserByExternalId;
using AridentIam.WebApi.Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AridentIam.WebApi.Controllers;

[ApiController]
[Route("api/v1/users")]
[Produces("application/json")]
public sealed class UsersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new user principal inside the specified tenant.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(
            TenantExternalId: request.TenantExternalId,
            FirstName: request.FirstName,
            LastName: request.LastName,
            DisplayName: request.DisplayName,
            Email: request.Email,
            Username: request.Username,
            PhoneNumber: request.PhoneNumber,
            JobTitle: request.JobTitle,
            EmploymentType: request.EmploymentType);

        var result = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            actionName: nameof(GetUserByExternalId),
            routeValues: new { userExternalId = result.UserExternalId },
            value: result);
    }

    /// <summary>
    /// Returns a user by external identifier.
    /// </summary>
    [HttpGet("{userExternalId:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByExternalId(
        Guid userExternalId,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByExternalIdQuery(userExternalId);
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}