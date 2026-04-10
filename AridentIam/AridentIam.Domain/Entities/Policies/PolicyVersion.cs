using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;
using AridentIam.Domain.Events;

namespace AridentIam.Domain.Entities.Policies;

public sealed class PolicyVersion : AggregateRoot
{
    private PolicyVersion() { }
    public Guid PolicyVersionExternalId { get; private set; }
    public Guid PolicyDefinitionExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public int VersionNumber { get; private set; }
    public VersionStatus Status { get; private set; }
    public PolicyEffectType EffectType { get; private set; }
    public string CombiningAlgorithm { get; private set; } = null!;
    public DateTimeOffset? PublishedAt { get; private set; }
    public DateTimeOffset? EffectiveFrom { get; private set; }
    public DateTimeOffset? EffectiveTo { get; private set; }

    public static PolicyVersion Create(Guid policyDefinitionExternalId, Guid tenantExternalId, int versionNumber, PolicyEffectType effectType, string combiningAlgorithm, DateTimeOffset? effectiveFrom, DateTimeOffset? effectiveTo, string createdBy)
    {
        Guard.AgainstInvalidRange(effectiveFrom, effectiveTo, nameof(effectiveTo));
        var entity = new PolicyVersion
        {
            PolicyVersionExternalId = Guid.NewGuid(),
            PolicyDefinitionExternalId = Guard.AgainstDefault(policyDefinitionExternalId, nameof(policyDefinitionExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            VersionNumber = versionNumber,
            Status = VersionStatus.Draft,
            EffectType = effectType,
            CombiningAlgorithm = Guard.AgainstNullOrWhiteSpace(combiningAlgorithm, nameof(combiningAlgorithm)),
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Publish(string updatedBy)
    {
        if (Status != VersionStatus.Draft)
            throw new DomainException("Only draft policy versions can be published.");
        Status = VersionStatus.Published;
        PublishedAt = DateTimeOffset.UtcNow;
        Touch(updatedBy);
        RaiseDomainEvent(new PolicyPublishedDomainEvent(PolicyVersionExternalId));
    }

    public void Supersede(string updatedBy)
    {
        if (Status != VersionStatus.Published)
            throw new DomainException("Only published policy versions can be superseded.");
        Status = VersionStatus.Superseded;
        Touch(updatedBy);
    }
}
