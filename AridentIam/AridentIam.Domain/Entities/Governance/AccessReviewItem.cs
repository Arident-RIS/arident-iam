using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Governance;

public sealed class AccessReviewItem : AuditableEntity
{
    private AccessReviewItem() { }
    public Guid AccessReviewItemExternalId { get; private set; }
    public Guid AccessReviewCampaignExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public Guid PrincipalExternalId { get; private set; }
    public Guid? RoleAssignmentExternalId { get; private set; }
    public Guid? DelegationGrantExternalId { get; private set; }
    public Guid? ReviewerPrincipalExternalId { get; private set; }
    public ReviewItemStatus Status { get; private set; }
    public ReviewDecision Decision { get; private set; }
    public DateTimeOffset? DecisionAt { get; private set; }

    public static AccessReviewItem Create(Guid accessReviewCampaignExternalId, Guid tenantExternalId, Guid principalExternalId, Guid? roleAssignmentExternalId, Guid? delegationGrantExternalId, Guid? reviewerPrincipalExternalId, string createdBy)
    {
        var entity = new AccessReviewItem
        {
            AccessReviewItemExternalId = Guid.NewGuid(),
            AccessReviewCampaignExternalId = Guard.AgainstDefault(accessReviewCampaignExternalId, nameof(accessReviewCampaignExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            PrincipalExternalId = Guard.AgainstDefault(principalExternalId, nameof(principalExternalId)),
            RoleAssignmentExternalId = roleAssignmentExternalId,
            DelegationGrantExternalId = delegationGrantExternalId,
            ReviewerPrincipalExternalId = reviewerPrincipalExternalId,
            Status = ReviewItemStatus.Pending,
            Decision = ReviewDecision.None
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Review(ReviewDecision decision, DateTimeOffset decisionAt, string updatedBy)
    {
        if (Status != ReviewItemStatus.Pending)
            throw new DomainException("Only pending review items can be reviewed.");
        if (decision == ReviewDecision.None)
            throw new DomainException("A review item must be resolved with a non-empty decision.");
        Decision = decision;
        DecisionAt = decisionAt;
        Status = ReviewItemStatus.Reviewed;
        Touch(updatedBy);
    }

    public void AutoClose(string updatedBy)
    {
        if (Status != ReviewItemStatus.Pending)
            throw new DomainException("Only pending review items can be auto-closed.");
        Status = ReviewItemStatus.AutoClosed;
        Touch(updatedBy);
    }
}