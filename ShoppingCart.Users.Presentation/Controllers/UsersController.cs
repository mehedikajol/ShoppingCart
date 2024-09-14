using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Users.Presentation.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("From users module");
    }
}
