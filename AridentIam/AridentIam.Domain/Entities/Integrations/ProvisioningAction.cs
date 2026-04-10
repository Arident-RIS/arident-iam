using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;
using AridentIam.Domain.Events;

namespace AridentIam.Domain.Entities.Integrations;

public sealed class ProvisioningAction : AggregateRoot
{
    private ProvisioningAction() { }
    public Guid ProvisioningActionExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public Guid ExternalConnectorExternalId { get; private set; }
    public ProvisioningActionType ActionType { get; private set; }
    public string ReferenceType { get; private set; } = null!;
    public string ReferenceId { get; private set; } = null!;
    public DateTimeOffset RequestedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public ProvisioningStatus Status { get; private set; }
    public string? ResultMessage { get; private set; }

    public static ProvisioningAction Create(Guid tenantExternalId, Guid externalConnectorExternalId, ProvisioningActionType actionType, string referenceType, string referenceId, DateTimeOffset requestedAt, string createdBy)
    {
        var entity = new ProvisioningAction
        {
            ProvisioningActionExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            ExternalConnectorExternalId = Guard.AgainstDefault(externalConnectorExternalId, nameof(externalConnectorExternalId)),
            ActionType = actionType,
            ReferenceType = Guard.AgainstNullOrWhiteSpace(referenceType, nameof(referenceType)),
            ReferenceId = Guard.AgainstNullOrWhiteSpace(referenceId, nameof(referenceId)),
            RequestedAt = requestedAt,
            Status = ProvisioningStatus.Pending
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Start(string updatedBy)
    {
        if (Status != ProvisioningStatus.Pending)
            throw new DomainException("Only pending provisioning actions can be started.");
        Status = ProvisioningStatus.InProgress;
        Touch(updatedBy);
    }

    public void CompleteSuccessfully(DateTimeOffset completedAt, string updatedBy, string? resultMessage = null)
    {
        if (Status != ProvisioningStatus.InProgress)
            throw new DomainException("Only in-progress provisioning actions can be completed.");
        if (completedAt < RequestedAt)
            throw new DomainException("Provisioning completion time cannot be earlier than request time.");
        CompletedAt = completedAt;
        Status = ProvisioningStatus.Succeeded;
        ResultMessage = string.IsNullOrWhiteSpace(resultMessage) ? null : resultMessage.Trim();
        Touch(updatedBy);
    }

    public void Fail(DateTimeOffset completedAt, string resultMessage, string updatedBy)
    {
        if (Status != ProvisioningStatus.InProgress)
            throw new DomainException("Only in-progress provisioning actions can be failed.");
        if (completedAt < RequestedAt)
            throw new DomainException("Provisioning completion time cannot be earlier than request time.");
        CompletedAt = completedAt;
        Status = ProvisioningStatus.Failed;
        ResultMessage = Guard.AgainstNullOrWhiteSpace(resultMessage, nameof(resultMessage));
        Touch(updatedBy);
        RaiseDomainEvent(new ProvisioningActionFailedDomainEvent(ProvisioningActionExternalId, TenantExternalId, ResultMessage));
    }
}