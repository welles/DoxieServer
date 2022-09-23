using Microsoft.AspNetCore.Mvc;

namespace DoxieServer.Controllers;

[ApiController]
[Route("/")]
public sealed class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return this.Content("Service is running.");
    }
}
