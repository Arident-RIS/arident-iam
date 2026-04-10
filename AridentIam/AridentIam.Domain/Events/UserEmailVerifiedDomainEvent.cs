using AridentIam.Domain.Common;

namespace AridentIam.Domain.Events;

public sealed record UserEmailVerifiedDomainEvent(Guid UserExternalId) : DomainEvent;
