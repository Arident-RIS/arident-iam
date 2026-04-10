using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Principals;

public sealed class PrincipalAttribute : AuditableEntity
{
    private PrincipalAttribute() { }
    public Guid PrincipalAttributeExternalId { get; private set; }
    public Guid PrincipalExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string AttributeName { get; private set; } = null!;
    public string AttributeValue { get; private set; } = null!;
    public string ValueType { get; private set; } = null!;
    public string Source { get; private set; } = null!;
    public bool IsVerified { get; private set; }

    public static PrincipalAttribute Create(Guid principalExternalId, Guid tenantExternalId, string attributeName, string attributeValue, string valueType, string source, bool isVerified, string createdBy)
    {
        var entity = new PrincipalAttribute
        {
            PrincipalAttributeExternalId = Guid.NewGuid(),
            PrincipalExternalId = Guard.AgainstDefault(principalExternalId, nameof(principalExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            AttributeName = Guard.AgainstNullOrWhiteSpace(attributeName, nameof(attributeName)),
            AttributeValue = Guard.AgainstNullOrWhiteSpace(attributeValue, nameof(attributeValue)),
            ValueType = Guard.AgainstNullOrWhiteSpace(valueType, nameof(valueType)),
            Source = Guard.AgainstNullOrWhiteSpace(source, nameof(source)),
            IsVerified = isVerified
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
