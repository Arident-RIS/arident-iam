using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;
using AridentIam.Domain.Events;

namespace AridentIam.Domain.Entities.Governance;

public sealed class AccessReviewCampaign : AggregateRoot
{
    private readonly List<AccessReviewItem> _items = [];

    private AccessReviewCampaign() { }

    public Guid AccessReviewCampaignExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string Name { get; private set; } = null!;
    public AccessReviewCampaignType CampaignType { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly DueDate { get; private set; }
    public GovernanceStatus Status { get; private set; }
    public Guid CreatedByPrincipalExternalId { get; private set; }

    public IReadOnlyCollection<AccessReviewItem> Items => _items.AsReadOnly();

    public static AccessReviewCampaign Create(Guid tenantExternalId, string name, AccessReviewCampaignType campaignType, DateOnly startDate, DateOnly dueDate, Guid createdByPrincipalExternalId, string createdBy)
    {
        if (dueDate < startDate)
            throw new DomainException("Access review due date cannot be earlier than start date.");

        var entity = new AccessReviewCampaign
        {
            AccessReviewCampaignExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name)),
            CampaignType = campaignType,
            StartDate = startDate,
            DueDate = dueDate,
            CreatedByPrincipalExternalId = Guard.AgainstDefault(createdByPrincipalExternalId, nameof(createdByPrincipalExternalId)),
            Status = GovernanceStatus.Draft
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Activate(string updatedBy)
    {
        if (Status != GovernanceStatus.Draft)
            throw new DomainException("Only draft campaigns can be activated.");
        Status = GovernanceStatus.Active;
        Touch(updatedBy);
        RaiseDomainEvent(new AccessReviewCampaignActivatedDomainEvent(AccessReviewCampaignExternalId, TenantExternalId));
    }

    public void Close(string updatedBy)
    {
        if (Status is GovernanceStatus.Closed or GovernanceStatus.Cancelled)
            throw new DomainException("Campaign is already closed or cancelled.");
        Status = GovernanceStatus.Closed;
        Touch(updatedBy);
    }

    public void Cancel(string updatedBy)
    {
        if (Status is GovernanceStatus.Closed or GovernanceStatus.Cancelled)
            throw new DomainException("Campaign is already closed or cancelled.");
        Status = GovernanceStatus.Cancelled;
        Touch(updatedBy);
    }

    public void AddItem(AccessReviewItem item)
    {
        Guard.AgainstNull(item, nameof(item));
        if (Status != GovernanceStatus.Active)
            throw new DomainException("Items can only be added to active campaigns.");
        _items.Add(item);
    }
}