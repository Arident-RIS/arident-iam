using FluentValidation;

namespace AridentIam.Application.Features.Organizations.Commands.CreateOrgSchema;

public sealed class CreateOrgSchemaCommandValidator : AbstractValidator<CreateOrgSchemaCommand>
{
    public CreateOrgSchemaCommandValidator()
    {
        RuleFor(x => x.TenantExternalId)
            .NotEmpty()
            .WithMessage("TenantExternalId is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Schema name is required.")
            .MaximumLength(200)
            .WithMessage("Schema name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description must not exceed 1000 characters.");
    }
}