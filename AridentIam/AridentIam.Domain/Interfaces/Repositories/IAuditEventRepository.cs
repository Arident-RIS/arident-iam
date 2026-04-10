using AridentIam.Domain.Entities.Auditing;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IAuditEventRepository
{
    Task AddAsync(AuditEvent entity, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AuditEvent>> GetByCorrelationIdAsync(Guid tenantExternalId, string correlationId, CancellationToken cancellationToken = default);
}
