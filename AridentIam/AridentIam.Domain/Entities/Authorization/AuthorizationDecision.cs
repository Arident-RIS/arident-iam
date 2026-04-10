using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Authorization;

public sealed class AuthorizationDecision : AggregateRoot
{
    private AuthorizationDecision() { }
    public Guid AuthorizationDecisionExternalId { get; private set; }
    public Guid AuthorizationRequestExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public DecisionType Decision { get; private set; }
    public string DecisionReasonCode { get; private set; } = null!;
    public Guid? MatchedPolicyVersionExternalId { get; private set; }
    public DateTime EvaluatedAtUtc { get; private set; }
    public long LatencyMs { get; private set; }
    public bool CacheHit { get; private set; }
    public string? DecisionTraceReference { get; private set; }

    public static AuthorizationDecision Create(Guid authorizationRequestExternalId, Guid tenantExternalId, DecisionType decision, string decisionReasonCode, Guid? matchedPolicyVersionExternalId, long latencyMs, bool cacheHit, string? decisionTraceReference, string createdBy)
    {
        var entity = new AuthorizationDecision
        {
            AuthorizationDecisionExternalId = Guid.NewGuid(),
            AuthorizationRequestExternalId = Guard.AgainstDefault(authorizationRequestExternalId, nameof(authorizationRequestExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            Decision = decision,
            DecisionReasonCode = Guard.AgainstNullOrWhiteSpace(decisionReasonCode, nameof(decisionReasonCode)),
            MatchedPolicyVersionExternalId = matchedPolicyVersionExternalId,
            EvaluatedAtUtc = DateTime.UtcNow,
            LatencyMs = latencyMs,
            CacheHit = cacheHit,
            DecisionTraceReference = string.IsNullOrWhiteSpace(decisionTraceReference) ? null : decisionTraceReference.Trim()
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
