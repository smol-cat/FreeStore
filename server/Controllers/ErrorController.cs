

using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

[ApiController]
[Route("api/v1/error")]
public class ErrorController : MainController
{
    [Route("")]
    public IActionResult Error()
    {
        return Problem();
    }

    [HttpGet]  
    [Route("cause")]
    public IActionResult CauseError()
    {
        throw(new Exception("lmao"));
    }
}