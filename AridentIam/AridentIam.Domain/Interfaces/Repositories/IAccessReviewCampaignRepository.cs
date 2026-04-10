using AridentIam.Domain.Entities.Governance;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IAccessReviewCampaignRepository
{
    Task<AccessReviewCampaign?> GetByExternalIdAsync(Guid accessReviewCampaignExternalId, CancellationToken cancellationToken = default);
    Task AddAsync(AccessReviewCampaign accessReviewCampaign, CancellationToken cancellationToken = default);
    void Update(AccessReviewCampaign accessReviewCampaign);
}