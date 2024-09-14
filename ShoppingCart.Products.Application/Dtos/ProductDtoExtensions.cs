using ShoppingCart.Products.Domain.Entities;

namespace ShoppingCart.Products.Application.Dtos;

public static class ProductDtoExtensions
{
    public static ProductDto ToProductDto(this Product product)
    {
        return new ProductDto(product.Id, product.Title, product.Description, product.Price);
    }
}
