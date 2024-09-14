namespace ShoppingCart.SharedKerel.Entities;

public interface IEntity<TId>
{
    TId Id { get; }
}
