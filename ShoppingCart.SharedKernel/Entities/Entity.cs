namespace ShoppingCart.SharedKerel.Entities;

public abstract class Entity : IEntity<Guid>
{
    public Guid Id { get; protected set; }
}
