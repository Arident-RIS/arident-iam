using AridentIam.Domain.Entities.Relationships;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IRelationshipTypeRepository
{
    Task<RelationshipType?> GetByExternalIdAsync(Guid relationshipTypeExternalId, CancellationToken cancellationToken = default);
    Task<RelationshipType?> GetByCodeAsync(Guid? tenantExternalId, string code, CancellationToken cancellationToken = default);
    Task AddAsync(RelationshipType relationshipType, CancellationToken cancellationToken = default);
    void Update(RelationshipType relationshipType);
}