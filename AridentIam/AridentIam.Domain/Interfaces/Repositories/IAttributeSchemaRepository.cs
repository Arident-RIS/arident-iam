using AridentIam.Domain.Entities.Attributes;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IAttributeSchemaRepository
{
    Task<AttributeSchema?> GetByExternalIdAsync(Guid attributeSchemaExternalId, CancellationToken cancellationToken = default);
    Task<AttributeSchema?> GetByNameAsync(Guid? tenantExternalId, string name, CancellationToken cancellationToken = default);
    Task AddAsync(AttributeSchema attributeSchema, CancellationToken cancellationToken = default);
    void Update(AttributeSchema attributeSchema);
}