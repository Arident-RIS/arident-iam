using AridentIam.Domain.Entities.Workflows;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IApprovalRequestRepository : IRepository<ApprovalRequest>
{
    Task<IReadOnlyList<ApprovalRequest>> GetPendingByTenantAsync(Guid tenantExternalId, CancellationToken cancellationToken = default);
}
