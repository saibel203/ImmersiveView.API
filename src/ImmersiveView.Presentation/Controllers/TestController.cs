using Microsoft.AspNetCore.Mvc;

namespace ImmersiveView.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("Test")]
    public IActionResult Test()
    {
        return Ok("HELLO!");
    }
}