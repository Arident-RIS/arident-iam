using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Events;

public sealed record SyncJobCompletedDomainEvent(
    Guid SyncJobExternalId,
    Guid ExternalConnectorExternalId,
    SyncJobOutcome Outcome,
    int ImportedCount,
    int FailedCount) : DomainEvent;
