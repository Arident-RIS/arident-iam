using AridentIam.Domain.Entities.Sessions;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface ISessionRepository : IRepository<Session>
{
    Task<IReadOnlyList<Session>> GetActiveByPrincipalAsync(Guid principalExternalId, CancellationToken cancellationToken = default);
}
