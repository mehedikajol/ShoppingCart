namespace ShoppingCart.SharedKerel.Entities;

public abstract class AuditableEntity : Entity
{
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}