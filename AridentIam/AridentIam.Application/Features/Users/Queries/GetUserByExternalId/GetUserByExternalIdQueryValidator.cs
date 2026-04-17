using FluentValidation;

namespace AridentIam.Application.Features.Users.Queries.GetUserByExternalId;

public sealed class GetUserByExternalIdQueryValidator : AbstractValidator<GetUserByExternalIdQuery>
{
    public GetUserByExternalIdQueryValidator()
    {
        RuleFor(x => x.TenantExternalId)
            .NotEmpty()
            .WithMessage("TenantExternalId is required.");

        RuleFor(x => x.UserExternalId)
            .NotEmpty()
            .WithMessage("UserExternalId is required.");
    }
}