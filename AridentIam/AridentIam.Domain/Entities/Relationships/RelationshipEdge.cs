using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Relationships;

public sealed class RelationshipEdge : AggregateRoot
{
    private RelationshipEdge() { }
    public Guid RelationshipEdgeExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public Guid RelationshipTypeExternalId { get; private set; }
    public Guid? SubjectPrincipalExternalId { get; private set; }
    public Guid? SubjectGroupExternalId { get; private set; }
    public Guid? ObjectPrincipalExternalId { get; private set; }
    public Guid? ObjectGroupExternalId { get; private set; }
    public Guid? ObjectOrganizationUnitExternalId { get; private set; }
    public Guid? ObjectResourceTypeExternalId { get; private set; }
    public Guid? ObjectResourceInstanceReferenceExternalId { get; private set; }
    public DateTimeOffset EffectiveFrom { get; private set; }
    public DateTimeOffset? EffectiveTo { get; private set; }
    public RelationshipStatus Status { get; private set; }

    public static RelationshipEdge Create(Guid tenantExternalId, Guid relationshipTypeExternalId, Guid? subjectPrincipalExternalId, Guid? subjectGroupExternalId, Guid? objectPrincipalExternalId, Guid? objectGroupExternalId, Guid? objectOrganizationUnitExternalId, Guid? objectResourceTypeExternalId, Guid? objectResourceInstanceReferenceExternalId, DateTimeOffset effectiveFrom, DateTimeOffset? effectiveTo, string createdBy)
    {
        EnsureSingleSubject(subjectPrincipalExternalId, subjectGroupExternalId);
        EnsureSingleObject(objectPrincipalExternalId, objectGroupExternalId, objectOrganizationUnitExternalId, objectResourceTypeExternalId, objectResourceInstanceReferenceExternalId);
        Guard.AgainstInvalidRange(effectiveFrom, effectiveTo, nameof(effectiveTo));

        var entity = new RelationshipEdge
        {
            RelationshipEdgeExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            RelationshipTypeExternalId = Guard.AgainstDefault(relationshipTypeExternalId, nameof(relationshipTypeExternalId)),
            SubjectPrincipalExternalId = subjectPrincipalExternalId,
            SubjectGroupExternalId = subjectGroupExternalId,
            ObjectPrincipalExternalId = objectPrincipalExternalId,
            ObjectGroupExternalId = objectGroupExternalId,
            ObjectOrganizationUnitExternalId = objectOrganizationUnitExternalId,
            ObjectResourceTypeExternalId = objectResourceTypeExternalId,
            ObjectResourceInstanceReferenceExternalId = objectResourceInstanceReferenceExternalId,
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo,
            Status = RelationshipStatus.Active
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Revoke(DateTimeOffset revokedAt, string updatedBy)
    {
        if (Status == RelationshipStatus.Revoked)
            throw new DomainException("Relationship is already revoked.");
        if (revokedAt < EffectiveFrom)
            throw new DomainException("Relationship revoke date cannot be earlier than effective start date.");
        EffectiveTo = revokedAt;
        Status = RelationshipStatus.Revoked;
        Touch(updatedBy);
    }

    public void Expire(DateTimeOffset expiredAt, string updatedBy)
    {
        if (Status != RelationshipStatus.Active)
            throw new DomainException("Only active relationships can be expired.");
        if (expiredAt < EffectiveFrom)
            throw new DomainException("Relationship expiry date cannot be earlier than effective start date.");
        EffectiveTo = expiredAt;
        Status = RelationshipStatus.Expired;
        Touch(updatedBy);
    }

    public void Activate(string updatedBy)
    {
        if (Status == RelationshipStatus.Revoked)
            throw new DomainException("A revoked relationship cannot be re-activated.");
        Status = RelationshipStatus.Active;
        Touch(updatedBy);
    }

    private static void EnsureSingleSubject(Guid? principalId, Guid? groupId)
    {
        var count = (principalId.HasValue ? 1 : 0) + (groupId.HasValue ? 1 : 0);
        if (count != 1)
            throw new DomainException("A relationship must have exactly one subject.");
    }

    private static void EnsureSingleObject(Guid? principalId, Guid? groupId, Guid? orgUnitId, Guid? resourceTypeId, Guid? resourceInstanceId)
    {
        var count = (principalId.HasValue ? 1 : 0) + (groupId.HasValue ? 1 : 0) +
                    (orgUnitId.HasValue ? 1 : 0) + (resourceTypeId.HasValue ? 1 : 0) +
                    (resourceInstanceId.HasValue ? 1 : 0);
        if (count != 1)
            throw new DomainException("A relationship must have exactly one object.");
    }
}