using ShoppingCart.Products.Application.Dtos;
using ShoppingCart.SharedKerel.Responses;

namespace ShoppingCart.Products.Application.Interfaces;

public interface IProductService
{
    Task<IResponse> GetAllProductsAsync();
    Task<IResponse> GetProductByIdAsync(Guid id);
    Task<IResponse> CreateProductAsync(ProductCreateDto dto);
    Task<IResponse> UpdateProductAsync(ProductUpdateDto dto);
    Task<IResponse> DeleteProductAsync(Guid id);
}
