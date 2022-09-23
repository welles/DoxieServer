using System.Reflection;
using DoxieServer.Core;
using Microsoft.AspNetCore.Mvc;

namespace DoxieServer.Controllers;

[ApiController]
[Route("/")]
public sealed class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return this.Content($"Service is running.\n\nBuild {Assembly.GetExecutingAssembly().GetVersion()} ({Assembly.GetExecutingAssembly().GetBuildDate().ToString("dd.MM.yyyy HH:mm:ss")})");
    }
}
