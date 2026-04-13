using FluentValidation;

namespace AridentIam.Application.Features.Organizations.Commands.CreateOrgUnitType;

public sealed class CreateOrgUnitTypeCommandValidator : AbstractValidator<CreateOrgUnitTypeCommand>
{
    public CreateOrgUnitTypeCommandValidator()
    {
        RuleFor(x => x.TenantExternalId)
            .NotEmpty();

        RuleFor(x => x.OrgSchemaExternalId)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.HierarchyLevel)
            .GreaterThan(0)
            .WithMessage("Hierarchy level must be greater than zero.");
    }
}