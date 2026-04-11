using AridentIam.Domain.Entities.Credentials;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface ICredentialRepository : IRepository<Credential>
{
    Task<IReadOnlyList<Credential>> GetActiveByPrincipalAsync(Guid principalExternalId, CancellationToken cancellationToken = default);
    Task<Credential?> GetByPrincipalAndTypeAsync(Guid principalExternalId, CredentialType credentialType, CancellationToken cancellationToken = default);
}