using AridentIam.Application.Features.Organizations.Commands.CreateOrgUnit;
using FluentAssertions;
using Xunit;

namespace AridentIam.Application.Tests.Features.Organizations.Commands.CreateOrgUnit;

public sealed class CreateOrgUnitCommandValidatorTests
{
    [Fact]
    public void Validate_Should_Fail_When_Code_Is_Empty()
    {
        var validator = new CreateOrgUnitCommandValidator();

        var command = new CreateOrgUnitCommand(
            TenantExternalId: Guid.NewGuid(),
            OrgSchemaExternalId: Guid.NewGuid(),
            OrgUnitTypeExternalId: Guid.NewGuid(),
            ParentOrganizationUnitExternalId: null,
            Code: string.Empty,
            Name: "Engineering");

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(CreateOrgUnitCommand.Code));
    }
}