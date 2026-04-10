using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Workflows;

public sealed class ApprovalRequest : AggregateRoot
{
    private ApprovalRequest() { }
    public Guid ApprovalRequestExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string RequestType { get; private set; } = null!;
    public string ReferenceType { get; private set; } = null!;
    public Guid? ReferenceId { get; private set; }
    public Guid RequestedByPrincipalExternalId { get; private set; }
    public ApprovalStatus CurrentStatus { get; private set; }
    public string Reason { get; private set; } = null!;
    public DateTime SubmittedAtUtc { get; private set; }
    public DateTime? ResolvedAtUtc { get; private set; }

    public static ApprovalRequest Create(Guid tenantExternalId, string requestType, string referenceType, Guid? referenceId, Guid requestedByPrincipalExternalId, string reason, string createdBy)
    {
        var entity = new ApprovalRequest
        {
            ApprovalRequestExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            RequestType = Guard.AgainstNullOrWhiteSpace(requestType, nameof(requestType)),
            ReferenceType = Guard.AgainstNullOrWhiteSpace(referenceType, nameof(referenceType)),
            ReferenceId = referenceId,
            RequestedByPrincipalExternalId = Guard.AgainstDefault(requestedByPrincipalExternalId, nameof(requestedByPrincipalExternalId)),
            CurrentStatus = ApprovalStatus.Pending,
            Reason = Guard.AgainstNullOrWhiteSpace(reason, nameof(reason)),
            SubmittedAtUtc = DateTime.UtcNow
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
