using AridentIam.Domain.Entities.Sessions;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IAuthenticationSessionRepository : IRepository<AuthenticationSession>
{
    Task<IReadOnlyList<AuthenticationSession>> GetOpenByPrincipalAsync(Guid principalExternalId, CancellationToken cancellationToken = default);
}