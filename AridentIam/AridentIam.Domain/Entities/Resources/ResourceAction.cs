using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Resources;

public sealed class ResourceAction : AuditableEntity
{
    private ResourceAction() { }
    public Guid ResourceActionExternalId { get; private set; }
    public Guid ResourceTypeExternalId { get; private set; }
    public string ActionKey { get; private set; } = null!;
    public string DisplayName { get; private set; } = null!;
    public string? Description { get; private set; }
    public string RiskLevel { get; private set; } = null!;

    public static ResourceAction Create(Guid resourceTypeExternalId, string actionKey, string displayName, string? description, string riskLevel, string createdBy)
    {
        var entity = new ResourceAction
        {
            ResourceActionExternalId = Guid.NewGuid(),
            ResourceTypeExternalId = Guard.AgainstDefault(resourceTypeExternalId, nameof(resourceTypeExternalId)),
            ActionKey = Guard.AgainstNullOrWhiteSpace(actionKey, nameof(actionKey)),
            DisplayName = Guard.AgainstNullOrWhiteSpace(displayName, nameof(displayName)),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            RiskLevel = Guard.AgainstNullOrWhiteSpace(riskLevel, nameof(riskLevel))
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
