using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Authorization;

public sealed class AuthorizationRequest : AggregateRoot
{
    private AuthorizationRequest() { }
    public Guid AuthorizationRequestExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string CorrelationId { get; private set; } = null!;
    public Guid PrincipalExternalId { get; private set; }
    public Guid? SessionExternalId { get; private set; }
    public string ActionKey { get; private set; } = null!;
    public Guid ResourceTypeExternalId { get; private set; }
    public Guid? ResourceInstanceReferenceExternalId { get; private set; }
    public string RequestContextJson { get; private set; } = null!;
    public DateTime RequestedAtUtc { get; private set; }

    public static AuthorizationRequest Create(Guid tenantExternalId, string correlationId, Guid principalExternalId, Guid? sessionExternalId, string actionKey, Guid resourceTypeExternalId, Guid? resourceInstanceReferenceExternalId, string requestContextJson, string createdBy)
    {
        var entity = new AuthorizationRequest
        {
            AuthorizationRequestExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            CorrelationId = Guard.AgainstNullOrWhiteSpace(correlationId, nameof(correlationId)),
            PrincipalExternalId = Guard.AgainstDefault(principalExternalId, nameof(principalExternalId)),
            SessionExternalId = sessionExternalId,
            ActionKey = Guard.AgainstNullOrWhiteSpace(actionKey, nameof(actionKey)),
            ResourceTypeExternalId = Guard.AgainstDefault(resourceTypeExternalId, nameof(resourceTypeExternalId)),
            ResourceInstanceReferenceExternalId = resourceInstanceReferenceExternalId,
            RequestContextJson = Guard.AgainstNullOrWhiteSpace(requestContextJson, nameof(requestContextJson)),
            RequestedAtUtc = DateTime.UtcNow
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
