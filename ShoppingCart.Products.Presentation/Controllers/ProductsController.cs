using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Products.Application.Interfaces;
using ShoppingCart.Products.Application.UseCases.Products.CreateProduct;

namespace ShoppingCart.Products.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly ISender _sender;

    public ProductsController(ILogger<ProductsController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await Task.Delay(10);
        // var response = await _productService.GetAllProductsAsync();
        return Ok("response");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        await Task.Delay(10);
        // var response = await _productService.GetAllProductsAsync();
        return Ok("response");
    }
}
