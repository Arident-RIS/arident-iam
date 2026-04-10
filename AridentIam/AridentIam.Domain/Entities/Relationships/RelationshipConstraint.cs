using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Relationships;

public sealed class RelationshipConstraint : AuditableEntity
{
    private RelationshipConstraint() { }
    public Guid RelationshipConstraintExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public Guid RelationshipTypeExternalId { get; private set; }
    public string ConstraintExpression { get; private set; } = null!;
    public string ConstraintFormat { get; private set; } = null!;

    public static RelationshipConstraint Create(Guid tenantExternalId, Guid relationshipTypeExternalId, string constraintExpression, string constraintFormat, string createdBy)
    {
        var entity = new RelationshipConstraint
        {
            RelationshipConstraintExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            RelationshipTypeExternalId = Guard.AgainstDefault(relationshipTypeExternalId, nameof(relationshipTypeExternalId)),
            ConstraintExpression = Guard.AgainstNullOrWhiteSpace(constraintExpression, nameof(constraintExpression)),
            ConstraintFormat = Guard.AgainstNullOrWhiteSpace(constraintFormat, nameof(constraintFormat))
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void UpdateExpression(string constraintExpression, string constraintFormat, string updatedBy)
    {
        ConstraintExpression = Guard.AgainstNullOrWhiteSpace(constraintExpression, nameof(constraintExpression));
        ConstraintFormat = Guard.AgainstNullOrWhiteSpace(constraintFormat, nameof(constraintFormat));
        Touch(updatedBy);
    }
}