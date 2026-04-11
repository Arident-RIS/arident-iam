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
    public string? ReviewerComment { get; private set; }
    public DateTimeOffset SubmittedAt { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }

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
            SubmittedAt = DateTimeOffset.UtcNow
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Approve(string updatedBy, string? reviewerComment = null)
    {
        EnsurePending();
        CurrentStatus = ApprovalStatus.Approved;
        ReviewerComment = string.IsNullOrWhiteSpace(reviewerComment) ? null : reviewerComment.Trim();
        ResolvedAt = DateTimeOffset.UtcNow;
        Touch(updatedBy);
    }

    public void Reject(string updatedBy, string? reviewerComment = null)
    {
        EnsurePending();
        CurrentStatus = ApprovalStatus.Rejected;
        ReviewerComment = string.IsNullOrWhiteSpace(reviewerComment) ? null : reviewerComment.Trim();
        ResolvedAt = DateTimeOffset.UtcNow;
        Touch(updatedBy);
    }

    public void Cancel(string updatedBy)
    {
        if (CurrentStatus is ApprovalStatus.Approved or ApprovalStatus.Rejected or ApprovalStatus.Cancelled)
            throw new DomainException("Approval request cannot be cancelled in its current state.");
        CurrentStatus = ApprovalStatus.Cancelled;
        ResolvedAt = DateTimeOffset.UtcNow;
        Touch(updatedBy);
    }

    public void Expire(string updatedBy)
    {
        EnsurePending();
        CurrentStatus = ApprovalStatus.Expired;
        ResolvedAt = DateTimeOffset.UtcNow;
        Touch(updatedBy);
    }

    private void EnsurePending()
    {
        if (CurrentStatus != ApprovalStatus.Pending)
            throw new DomainException("Only pending approval requests can be transitioned.");
    }
}
