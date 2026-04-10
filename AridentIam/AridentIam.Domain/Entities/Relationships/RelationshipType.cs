using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Relationships;

public sealed class RelationshipType : AggregateRoot
{
    private RelationshipType() { }
    public Guid RelationshipTypeExternalId { get; private set; }
    public Guid? TenantExternalId { get; private set; }
    public bool IsSystemOwned => !TenantExternalId.HasValue;
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public RelationshipObjectType SubjectType { get; private set; }
    public RelationshipObjectType ObjectType { get; private set; }
    public bool IsDelegable { get; private set; }
    public bool IsTransitive { get; private set; }

    public static RelationshipType Create(Guid? tenantExternalId, string code, string name, RelationshipObjectType subjectType, RelationshipObjectType objectType, bool isDelegable, bool isTransitive, string? description, string createdBy)
    {
        var entity = new RelationshipType
        {
            RelationshipTypeExternalId = Guid.NewGuid(),
            TenantExternalId = tenantExternalId,
            Code = Guard.AgainstNullOrWhiteSpace(code, nameof(code)),
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name)),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            SubjectType = subjectType,
            ObjectType = objectType,
            IsDelegable = isDelegable,
            IsTransitive = isTransitive
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void UpdateDetails(string name, string? description, string updatedBy)
    {
        Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Touch(updatedBy);
    }

    public void SetDelegation(bool isDelegable, string updatedBy)
    {
        IsDelegable = isDelegable;
        Touch(updatedBy);
    }
}