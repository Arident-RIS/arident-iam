using FluentValidation;

namespace AridentIam.Application.Features.Organizations.Commands.CreateOrgUnit;

public sealed class CreateOrgUnitCommandValidator : AbstractValidator<CreateOrgUnitCommand>
{
    public CreateOrgUnitCommandValidator()
    {
        RuleFor(x => x.TenantExternalId)
            .NotEmpty();

        RuleFor(x => x.OrgSchemaExternalId)
            .NotEmpty();

        RuleFor(x => x.OrgUnitTypeExternalId)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}