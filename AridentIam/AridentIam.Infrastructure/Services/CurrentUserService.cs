using AridentIam.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AridentIam.Infrastructure.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public string UserId =>
        User?.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? User?.FindFirstValue("sub")
        ?? "anonymous";

    public string? TenantId =>
        User?.FindFirstValue("tid")
        ?? User?.FindFirstValue("tenant_id");

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated ?? false;

    public string ActorIdentifier => IsAuthenticated ? UserId : "system";
}
