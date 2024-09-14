using ShoppingCart.Products.Application.Dtos;
using ShoppingCart.Products.Application.Interfaces;
using ShoppingCart.Products.Contracts;
using ShoppingCart.Products.Domain.Entities;
using ShoppingCart.SharedKerel.Repositories;
using ShoppingCart.SharedKerel.Responses;
using ShoppingCart.SharedKernel.Events;
using ShoppingCart.SharedKernel.Exceptions;

namespace ShoppingCart.Products.Infrastructure.Services;

internal class ProductService : IProductService
{
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IEventDispatcher _eventDispatcher;

    public ProductService(IRepository<Product, Guid> productRepository, IEventDispatcher eventDispatcher)
    {
        _productRepository = productRepository;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<IResponse> GetAllProductsAsync()
    {
        try
        {
            var products = await _productRepository.GetAllAsync();
            var dtos = products
                .Select(p => p.ToProductDto())
                .ToList();
            return new SuccessResponse(dtos, "Products fetching success", 200);
        }
        catch (Exception ex)
        {
            // log the error message
            Console.WriteLine(ex.Message);

            return new ErrorResponse("Error fetching products", 500);
        }
    }

    public async Task<IResponse> GetProductByIdAsync(Guid id)
    {
        try
        {
            var product = await _productRepository.GetAsync(id);
            if (product is null)
            {
                return new ErrorResponse("Product not found", 404);
            }

            var dto = product.ToProductDto();
            return new SuccessResponse(dto, "Product fetching success", 200);
        }
        catch (Exception ex)
        {
            // log the error message
            Console.WriteLine(ex.Message);

            return new ErrorResponse("Error fetching product", 500);
        }
    }

    public async Task<IResponse> CreateProductAsync(ProductCreateDto dto)
    {
        try
        {
            var product = Product.Create(dto.Title, dto.Description, dto.Price);
            await _productRepository.CreateAsync(product);

            _eventDispatcher.Publish(new ProductCreatedEvent(product.Id, product.Title, product.Description, product.Price));

            return new SuccessResponse(null, "Products adding success", 200);
        }
        catch (Exception ex)
        {
            // log the error message
            Console.WriteLine(ex.Message);

            if (ex is DomainValidationException)
            {
                return new ValidationErrorResponse(new List<string> { ex.Message });
            }

            return new ErrorResponse("Error adding error", 500);
        }
    }

    public async Task<IResponse> UpdateProductAsync(ProductUpdateDto dto)
    {
        try
        {
            var productToUpdate = await _productRepository.GetAsync(dto.Id);
            if (productToUpdate is null)
            {
                return new ErrorResponse("Product not found", 404);
            }

            productToUpdate.Update(dto.Title, dto.Description, dto.Price);
            await _productRepository.UpdateAsync(productToUpdate);

            _eventDispatcher.Publish(new ProductUpdatedEvent(productToUpdate.Id, productToUpdate.Title, productToUpdate.Description, productToUpdate.Price));

            return new SuccessResponse(null, "Products updating success", 200);
        }
        catch (Exception ex)
        {
            // log the error message
            Console.WriteLine(ex.Message);

            if (ex is DomainValidationException)
            {
                return new ValidationErrorResponse(new List<string> { ex.Message });
            }

            return new ErrorResponse("Error fetching products", 500);
        }
    }

    public async Task<IResponse> DeleteProductAsync(Guid id)
    {
        try
        {
            var productToDelete = await _productRepository.GetAsync(id);
            if (productToDelete is null)
            {
                return new ErrorResponse("Product not found", 404);
            }

            await _productRepository.DeleteAsync(id);

            _eventDispatcher.Publish(new ProductDeletedEvent(productToDelete.Id));

            return new SuccessResponse(null, "Products deleting success", 200);
        }
        catch (Exception ex)
        {
            // log the error message
            Console.WriteLine(ex.Message);

            return new ErrorResponse("Error deleting products", 500);
        }
    }
}
