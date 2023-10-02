using Microsoft.AspNetCore.Mvc;

namespace I18nApi.Api.Controllers;

[Route("[controller]")]
public class HealthController:ControllerBase
{
    [HttpGet("[action]")]
    public IActionResult Check() {
        return Ok();
    }
}