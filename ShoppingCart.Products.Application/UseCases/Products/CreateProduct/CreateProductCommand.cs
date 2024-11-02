using MediatR;
using ShoppingCart.Products.Application.Dtos;
using ShoppingCart.Products.Application.Interfaces;
using ShoppingCart.SharedKerel.Responses;

namespace ShoppingCart.Products.Application.UseCases.Products.CreateProduct;

public record CreateProductCommand(string Title, string Description, decimal Price) : IRequest<IResponse>;

internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, IResponse>
{
    private readonly IProductService _productService;

    public CreateProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public Task<IResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productCreateDto = new ProductCreateDto(request.Title, request.Description, request.Price);
        var response = _productService.CreateProductAsync(productCreateDto);
        return response;
    }
}