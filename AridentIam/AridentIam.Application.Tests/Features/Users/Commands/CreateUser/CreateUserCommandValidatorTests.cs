using AridentIam.Application.Features.Users.Commands.CreateUser;
using AridentIam.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace AridentIam.Application.Tests.Features.Users.Commands.CreateUser;

public sealed class CreateUserCommandValidatorTests
{
    [Fact]
    public void Validate_Should_Fail_When_Email_Is_Invalid()
    {
        var validator = new CreateUserCommandValidator();

        var command = new CreateUserCommand(
            TenantExternalId: Guid.NewGuid(),
            FirstName: "Abhishek",
            LastName: "Patil",
            DisplayName: "Abhishek Patil",
            Email: "invalid-email",
            Username: "abhishek.patil",
            PhoneNumber: "+919999999999",
            JobTitle: "Software Engineer",
            EmploymentType: EmploymentType.Employee);

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(CreateUserCommand.Email));
    }
}