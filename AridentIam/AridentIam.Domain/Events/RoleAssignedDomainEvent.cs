using AridentIam.Domain.Common;

namespace AridentIam.Domain.Events;

public sealed record RoleAssignedDomainEvent(Guid RoleAssignmentExternalId, Guid RoleDefinitionExternalId) : DomainEvent;
