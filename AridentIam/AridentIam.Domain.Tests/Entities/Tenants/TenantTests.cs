using AridentIam.Domain.Entities.Tenants;
using AridentIam.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace AridentIam.Domain.Tests.Entities.Tenants;

public sealed class TenantTests
{
    [Fact]
    public void Create_Should_Set_Core_Properties()
    {
        var tenantExternalId = Guid.NewGuid();

        var tenant = Tenant.Create(
            tenantExternalId: tenantExternalId,
            code: "DEFAULT",
            name: "Default Tenant",
            isolationMode: IsolationMode.Shared,
            createdBy: "system");

        tenant.TenantExternalId.Should().Be(tenantExternalId);
        tenant.Code.Should().Be("DEFAULT");
        tenant.Name.Should().Be("Default Tenant");
        tenant.IsolationMode.Should().Be(IsolationMode.Shared);
    }
}