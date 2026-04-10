using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Integrations;

public sealed class ExternalConnector : AggregateRoot
{
    private ExternalConnector() { }
    public Guid ExternalConnectorExternalId { get; private set; }
    public Guid? TenantExternalId { get; private set; }
    public bool IsSystemOwned => !TenantExternalId.HasValue;
    public Guid ExternalSystemExternalId { get; private set; }
    public string ConnectorType { get; private set; } = null!;
    public string ConfigurationReference { get; private set; } = null!;
    public string? AuthenticationReference { get; private set; }
    public string SyncMode { get; private set; } = null!;
    public ExternalConnectorStatus Status { get; private set; }

    public static ExternalConnector Create(Guid? tenantExternalId, Guid externalSystemExternalId, string connectorType, string configurationReference, string syncMode, string? authenticationReference, string createdBy)
    {
        var entity = new ExternalConnector
        {
            ExternalConnectorExternalId = Guid.NewGuid(),
            TenantExternalId = tenantExternalId,
            ExternalSystemExternalId = Guard.AgainstDefault(externalSystemExternalId, nameof(externalSystemExternalId)),
            ConnectorType = Guard.AgainstNullOrWhiteSpace(connectorType, nameof(connectorType)),
            ConfigurationReference = Guard.AgainstNullOrWhiteSpace(configurationReference, nameof(configurationReference)),
            SyncMode = Guard.AgainstNullOrWhiteSpace(syncMode, nameof(syncMode)),
            AuthenticationReference = string.IsNullOrWhiteSpace(authenticationReference) ? null : authenticationReference.Trim(),
            Status = ExternalConnectorStatus.Active
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Disable(string updatedBy) { Status = ExternalConnectorStatus.Disabled; Touch(updatedBy); }
    public void MarkError(string updatedBy) { Status = ExternalConnectorStatus.Error; Touch(updatedBy); }
    public void Activate(string updatedBy) { Status = ExternalConnectorStatus.Active; Touch(updatedBy); }
}