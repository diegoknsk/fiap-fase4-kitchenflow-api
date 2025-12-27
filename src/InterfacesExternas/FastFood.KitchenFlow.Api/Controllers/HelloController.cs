using Microsoft.AspNetCore.Mvc;

namespace FastFood.KitchenFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "Olá Mundo - KitchenFlow API está funcionando!" });
    }
}

