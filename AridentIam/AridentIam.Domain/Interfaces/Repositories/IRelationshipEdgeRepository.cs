using AridentIam.Domain.Entities.Relationships;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IRelationshipEdgeRepository
{
    Task<RelationshipEdge?> GetByExternalIdAsync(Guid relationshipEdgeExternalId, CancellationToken cancellationToken = default);
    Task AddAsync(RelationshipEdge relationshipEdge, CancellationToken cancellationToken = default);
    void Update(RelationshipEdge relationshipEdge);
}