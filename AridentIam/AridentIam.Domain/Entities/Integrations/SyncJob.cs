using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;
using AridentIam.Domain.Events;

namespace AridentIam.Domain.Entities.Integrations;

public sealed class SyncJob : AggregateRoot
{
    private SyncJob() { }
    public Guid SyncJobExternalId { get; private set; }
    public Guid? TenantExternalId { get; private set; }
    public bool IsSystemOwned => !TenantExternalId.HasValue;
    public Guid ExternalConnectorExternalId { get; private set; }
    public SyncJobType JobType { get; private set; }
    public DateTimeOffset StartedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public SyncJobOutcome Outcome { get; private set; }
    public int ImportedCount { get; private set; }
    public int FailedCount { get; private set; }

    public static SyncJob Create(Guid? tenantExternalId, Guid externalConnectorExternalId, SyncJobType jobType, DateTimeOffset startedAt, string createdBy)
    {
        var entity = new SyncJob
        {
            SyncJobExternalId = Guid.NewGuid(),
            TenantExternalId = tenantExternalId,
            ExternalConnectorExternalId = Guard.AgainstDefault(externalConnectorExternalId, nameof(externalConnectorExternalId)),
            JobType = jobType,
            StartedAt = startedAt,
            Outcome = SyncJobOutcome.Pending
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Complete(SyncJobOutcome outcome, int importedCount, int failedCount, DateTimeOffset completedAt, string updatedBy)
    {
        if (completedAt < StartedAt)
            throw new DomainException("Sync job completion time cannot be earlier than start time.");
        Guard.AgainstNegative(importedCount, nameof(importedCount));
        Guard.AgainstNegative(failedCount, nameof(failedCount));

        Outcome = outcome;
        ImportedCount = importedCount;
        FailedCount = failedCount;
        CompletedAt = completedAt;
        Touch(updatedBy);
        RaiseDomainEvent(new SyncJobCompletedDomainEvent(SyncJobExternalId, ExternalConnectorExternalId, Outcome, ImportedCount, FailedCount));
    }
}