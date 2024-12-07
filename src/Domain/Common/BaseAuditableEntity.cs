namespace DataVision.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; } = DateTimeOffset.UtcNow;

    public string? LastModifiedBy { get; set; }

    protected void Update(string? userId)
    {
        LastModifiedBy = userId;
        LastModified = DateTimeOffset.UtcNow;
    }
}
