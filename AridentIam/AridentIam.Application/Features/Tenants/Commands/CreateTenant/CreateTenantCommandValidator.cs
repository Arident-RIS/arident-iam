using FluentValidation;

namespace AridentIam.Application.Features.Tenants.Commands.CreateTenant;

public sealed class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Tenant code is required.")
            .MaximumLength(100)
            .WithMessage("Tenant code must not exceed 100 characters.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Tenant name is required.")
            .MaximumLength(200)
            .WithMessage("Tenant name must not exceed 200 characters.");

        RuleFor(x => x.IsolationMode)
            .IsInEnum()
            .WithMessage("A valid isolation mode is required.");

        RuleFor(x => x.DefaultLocale)
            .MaximumLength(20)
            .WithMessage("Default locale must not exceed 20 characters.");

        RuleFor(x => x.DefaultTimeZone)
            .MaximumLength(100)
            .WithMessage("Default time zone must not exceed 100 characters.");
    }
}