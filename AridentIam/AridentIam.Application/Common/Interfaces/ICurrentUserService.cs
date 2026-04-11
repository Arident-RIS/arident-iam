namespace AridentIam.Application.Common.Interfaces;

public interface ICurrentUserService
{
    /// <summary>Subject claim (user ID) from the JWT, or "anonymous" when unauthenticated.</summary>
    string UserId { get; }

    /// <summary>Tenant identifier extracted from the JWT's "tid" claim.</summary>
    string? TenantId { get; }

    bool IsAuthenticated { get; }

    /// <summary>Returns <see cref="UserId"/> when authenticated, otherwise "system".</summary>
    string ActorIdentifier { get; }
}
