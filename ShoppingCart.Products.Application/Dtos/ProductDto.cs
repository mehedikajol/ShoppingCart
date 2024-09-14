namespace ShoppingCart.Products.Application.Dtos;

public record ProductDto(Guid Id, string Title, string Description, decimal Price);
