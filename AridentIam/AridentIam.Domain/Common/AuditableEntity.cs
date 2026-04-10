namespace AridentIam.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
    public string CreatedBy { get; protected set; } = "system";
    public DateTimeOffset? UpdatedAt { get; protected set; }
    public string? UpdatedBy { get; protected set; }

    protected void SetCreationAudit(string createdBy)
    {
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? "system" : createdBy.Trim();
        CreatedAt = DateTimeOffset.UtcNow;
    }

    protected void Touch(string updatedBy)
    {
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? "system" : updatedBy.Trim();
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
