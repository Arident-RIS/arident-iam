using AridentIam.Domain.Entities.Authorization;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IAuthorizationDecisionRepository : IRepository<AuthorizationDecision>
{
    Task<IReadOnlyList<AuthorizationDecision>> GetByCorrelationIdAsync(string correlationId, CancellationToken cancellationToken = default);
}
