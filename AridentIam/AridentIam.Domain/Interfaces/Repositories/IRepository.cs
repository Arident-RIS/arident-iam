namespace AridentIam.Domain.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<T?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Remove(T entity);
}
