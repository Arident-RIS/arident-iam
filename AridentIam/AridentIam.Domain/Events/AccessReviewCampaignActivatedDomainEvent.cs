using AridentIam.Domain.Common;

namespace AridentIam.Domain.Events;

public sealed record AccessReviewCampaignActivatedDomainEvent(
    Guid AccessReviewCampaignExternalId,
    Guid TenantExternalId) : DomainEvent;
